using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Clients.ITSync.Models;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Newtonsoft.Json;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;
using Grid.Features.IMS.ViewModels;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.IMS.Controllers
{
    public class AssetsController : InventoryBaseController
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetAllocationRepository _assetAllocationRepository;
        private readonly IAssetDocumentRepository _assetDocumentRepository;
        private readonly ISystemSnapshotRepository _systemSnapshotRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IVendorRepository _vendorRepository;

        private readonly EmailComposerService _emailComposerService;

        public AssetsController(IAssetRepository assetRepository,
                                IAssetAllocationRepository assetAllocationRepository,
                                IAssetDocumentRepository assetDocumentRepository,
                                ISystemSnapshotRepository systemSnapshotRepository,
                                IUnitOfWork unitOfWork,
                                IUserRepository userRepository,
                                IAssetCategoryRepository assetCategoryRepository,
                                IDepartmentRepository departmentRepository,
                                IVendorRepository vendorRepository,
                                IEmployeeRepository employeeRepository,
                                EmailComposerService emailComposerService)
        {
            _assetRepository = assetRepository;
            _assetAllocationRepository = assetAllocationRepository;
            _assetDocumentRepository = assetDocumentRepository;
            _systemSnapshotRepository = systemSnapshotRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _assetCategoryRepository = assetCategoryRepository;
            _departmentRepository = departmentRepository;
            _vendorRepository = vendorRepository;
            _employeeRepository = employeeRepository;
            _emailComposerService = emailComposerService;
        }

        [SelectList("User", "AllocatedUserId")]
        [SelectList("Vendor", "VendorId")]
        [SelectList("AssetCategory", "CategoryId")]
        [SelectList("Department", "DepartmentId")]      
        public ActionResult Index()
        {
            var asset = _assetRepository.GetAll();
            return View(asset.ToList());
        }

        public ActionResult Details(int id)
        {
            var asset = _assetRepository.Get(id, "AllocatedUser.Person");

            if (asset == null)
            {
                return HttpNotFound();
            }

            var allocations = _assetAllocationRepository.GetAllBy(a => a.AssetId == asset.Id, "AllocatedUser.Person,AllocatedByUser.Person").ToList();
            var vm = new AssetDetailsViewModel(asset)
            {
                AssetAllocations = allocations
            };

            // If we have snapshots, set snapshot variable
            var snapshot = _systemSnapshotRepository.GetBy(s => s.AssetId == asset.Id);
            if (snapshot != null)
            {
                vm.HasSoftwareInfo = true;
                vm.Softwares = JsonConvert.DeserializeObject<List<SoftwareModel>>(snapshot.Softwares);

                vm.HasHardwareInfo = true;
                vm.Hardware = JsonConvert.DeserializeObject<HardwareModel>(snapshot.Hardwares);
            }

            var assetDocs = _assetDocumentRepository.GetAllBy(a => a.AssetId == asset.Id);
            vm.AssetDocuments = assetDocs.ToList();

            return View(vm);
        }

        [SelectList("User", "AllocatedUserId")]
        [SelectList("Vendor", "VendorId")]
        [SelectList("AssetCategory", "AssetCategoryId")]
        [SelectList("Department", "DepartmentId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("User", "AllocatedUserId", SelectListState.Recreate)]
        [SelectList("Vendor", "VendorId", SelectListState.Recreate)]
        [SelectList("AssetCategory", "AssetCategoryId", SelectListState.Recreate)]
        [SelectList("Department", "DepartmentId", SelectListState.Recreate)]
        public ActionResult Create(Asset asset)
        {
            if (ModelState.IsValid)
            {
                // Asset Title is Mandatory
                if (string.IsNullOrEmpty(asset.Title))
                {
                    asset.Title = "No Title";
                }

                _assetRepository.Create(asset);
                _unitOfWork.Commit();

                // Update Tag Number 
                var selectedAsset = _assetRepository.Get(asset.Id);
                if (selectedAsset != null)
                {
                    selectedAsset.TagNumber = $"LA{asset.Id.ToString("D" + 6)}";
                    _assetRepository.Update(selectedAsset);
                    _unitOfWork.Commit();
                }

                #if !DEBUG
                if(selectedAsset != null)
                    _emailComposerService.AssetStateChanged(selectedAsset.Id);
                #endif

                return RedirectToAction("Index");
            }

            return View(asset);
        }

        public ActionResult Edit(int id)
        {
            var asset = _assetRepository.Get(id);

            ViewBag.AllocatedUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", asset.AllocatedEmployeeId);
            ViewBag.AssetCategoryId = new SelectList(_assetCategoryRepository.GetAll(), "Id", "Title", asset.AssetCategoryId);
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Title", asset.DepartmentId);
            ViewBag.VendorId = new SelectList(_vendorRepository.GetAll(), "Id", "Title", asset.VendorId);

            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("User", "AllocatedUserId", SelectListState.Recreate)]
        [SelectList("Vendor", "VendorId", SelectListState.Recreate)]
        [SelectList("AssetCategory", "AssetCategoryId", SelectListState.Recreate)]
        [SelectList("Department", "DepartmentId", SelectListState.Recreate)]
        public ActionResult Edit(Asset asset)
        {
            if (ModelState.IsValid)
            {
                var selectedAsset = _assetRepository.Get(asset.Id);

                if (selectedAsset != null)
                {
                    // Log new Changes in Allocation Table
                    if (selectedAsset.State != asset.State || selectedAsset.AllocatedEmployeeId != asset.AllocatedEmployeeId)
                    {
                        var assetAllocation = new AssetAllocation
                        {
                            State = asset.State,
                            AllocatedEmployeeId = asset.AllocatedEmployeeId,
                            AllocatedOn = DateTime.UtcNow,
                            AllocatedByEmployeeId = WebUser.Id,
                            AssetId = selectedAsset.Id
                        };

                        _assetAllocationRepository.Create(assetAllocation);
                        _unitOfWork.Commit();

                        // Asset State Changed
                        #if !DEBUG
                            _emailComposerService.AssetStateChanged(selectedAsset.Id);
                        #endif
                    }

                    // Update main Asset

                    selectedAsset.SerialNumber = asset.SerialNumber;
                    selectedAsset.Title = asset.Title;
                    selectedAsset.Description = asset.Description;
                    selectedAsset.Specifications = asset.Specifications;
                    selectedAsset.Brand = asset.Brand;
                    selectedAsset.Cost = asset.Cost;
                    selectedAsset.ModelNumber = asset.ModelNumber;
                    selectedAsset.IsBrandNew = asset.IsBrandNew;
                    selectedAsset.State = asset.State;
                    selectedAsset.PurchaseDate = asset.PurchaseDate;
                    selectedAsset.WarrantyExpiryDate = asset.WarrantyExpiryDate;
                    selectedAsset.AssetCategoryId = asset.AssetCategoryId;
                    selectedAsset.DepartmentId = asset.DepartmentId;
                    selectedAsset.VendorId = asset.VendorId;
                    selectedAsset.AllocatedEmployeeId = asset.AllocatedEmployeeId;

                    _assetRepository.Update(selectedAsset);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }

            return View(asset);
        }

        public ActionResult Delete(int id)
        {
            var award = _assetRepository.Get(id);
            return CheckForNullAndExecute(award, () => View(award));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _assetRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult MyAssets()
        {
            var employeeId = _employeeRepository.GetBy(u => u.UserId == WebUser.Id,"User");
            ViewBag.employeeId = employeeId.Id;
            return View();
        }
    }
}
