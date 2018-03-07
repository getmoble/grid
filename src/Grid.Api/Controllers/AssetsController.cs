using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;
using Grid.Features.EmailService;
using System.Linq;
using Grid.Api.Models.IMS;
using System;
using Grid.Features.HRMS.DAL.Interfaces;
using System.IO;
using Grid.Data;

namespace Grid.Api.Controllers
{
    public class AssetsController : GridApiBaseController
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailComposerService _emailComposerService;
        private readonly IAssetAllocationRepository _assetAllocationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly GridDataContext _dataContext;


        public AssetsController(IAssetRepository assetRepository, EmailComposerService emailComposerService, IAssetAllocationRepository assetAllocationRepository, IEmployeeRepository employeeRepository,
                                   IUnitOfWork unitOfWork)
        {
            _assetRepository = assetRepository;
            _assetAllocationRepository = assetAllocationRepository;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
            _emailComposerService = emailComposerService;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _assetRepository.GetAll().Select(h => new AssetModel(h)).ToList();
            }, "Assets Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _assetRepository.Get(id, "AllocatedEmployee.User.Person"), "Assets fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
              
        [HttpPost]

        public JsonResult Update(AssetModel assetModel)
        {
            ApiResult<Asset> apiResult;

            if (ModelState.IsValid)
            {
                if (assetModel.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedAsset = _assetRepository.Get(assetModel.Id);
                        if (selectedAsset != null)
                        {
                            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id);

                            // Log new Changes in Allocation Table
                            if (selectedAsset.State != assetModel.State || selectedAsset.AllocatedEmployeeId != assetModel.AllocatedEmployeeId)
                            {
                                var assetAllocation = new AssetAllocation
                                {
                                    State = assetModel.State,
                                    AllocatedEmployeeId = assetModel.AllocatedEmployeeId,
                                    AllocatedOn = DateTime.UtcNow,
                                    AllocatedByEmployeeId = employee.Id,
                                    AssetId = selectedAsset.Id
                                };
                               
                                _assetAllocationRepository.Create(assetAllocation);
                                _unitOfWork.Commit();


                            }
                            // Update main Asset

                            selectedAsset.SerialNumber = assetModel.SerialNumber;
                            selectedAsset.Title = assetModel.Title;
                            selectedAsset.Description = assetModel.Description;
                            selectedAsset.Specifications = assetModel.Specifications;
                            selectedAsset.Brand = assetModel.Brand;
                            selectedAsset.Cost = assetModel.Cost;
                            selectedAsset.TagNumber = assetModel.TagNumber;
                            selectedAsset.ModelNumber = assetModel.ModelNumber;
                            selectedAsset.IsBrandNew = assetModel.IsBrandNew;
                            selectedAsset.State = assetModel.State;
                            selectedAsset.PurchaseDate = assetModel.PurchaseDate;
                            selectedAsset.WarrantyExpiryDate = assetModel.WarrantyExpiryDate;
                            selectedAsset.AssetCategoryId = assetModel.AssetCategoryId;
                            selectedAsset.DepartmentId = assetModel.DepartmentId;
                            selectedAsset.VendorId = assetModel.VendorId;
                            selectedAsset.AllocatedEmployeeId = assetModel.AllocatedEmployeeId;

                            _assetRepository.Update(selectedAsset);
                            _unitOfWork.Commit();

                        }
                       
                        // Asset State Changed
#if !DEBUG
                                _emailComposerService.AssetStateChanged(selectedAsset.Id);
#endif

                        return selectedAsset;

                    }, "Asset updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var asset = new Asset
                        {
                            Title = assetModel.Title,
                            SerialNumber = assetModel.SerialNumber,
                            Description = assetModel.Description,
                            Specifications = assetModel.Specifications,
                            Brand = assetModel.Brand,
                            Cost = assetModel.Cost,
                            ModelNumber = assetModel.ModelNumber,
                            IsBrandNew = assetModel.IsBrandNew,
                            State = assetModel.State,
                            PurchaseDate = assetModel.PurchaseDate,
                            WarrantyExpiryDate = assetModel.WarrantyExpiryDate,
                            AssetCategoryId = assetModel.AssetCategoryId,
                            DepartmentId = assetModel.DepartmentId,
                            VendorId = assetModel.VendorId,
                            AllocatedEmployeeId = assetModel.AllocatedEmployeeId,
                            Id = assetModel.Id
                        };

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
                        if (selectedAsset != null)
                            _emailComposerService.AssetStateChanged(selectedAsset.Id);
#endif
                        return asset;
                    }, "Asset created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Asset>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportCsv()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"Serial Number\",\"TagNumber\",\"Title\",\"Description\",\"Specifications\",\"Brand\",\"Cost\",\"Model Number\",\"Is BrandNew\",\"State\",\"Purchase Date\",\"Warranty Expiry Date\",\"Asset Category\",\"Department\",\"Vendor\",\"Allocated Employee\",\"Created On\"");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=AssetDetails.csv");
            Response.ContentType = "text/csv";

            var getAllAssets = _dataContext.Assets.Include("AllocatedEmployee").Include("AllocatedEmployee.User").Include("AllocatedEmployee.User.Person").Include("Department").Include("Vendor").Include("AssetCategory").ToList(); ;


            if (getAllAssets != null)
            {
                foreach (var asset in getAllAssets)
                {

                    sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\"", asset.SerialNumber, asset.TagNumber, asset.Title, asset.Description, asset.Specifications, asset.Brand, asset.Cost, asset.ModelNumber, asset.IsBrandNew, asset.State, asset.PurchaseDate.ToString(), asset.WarrantyExpiryDate, asset.AssetCategory.Title, asset.Department.Title, asset.Vendor.Title, asset.AllocatedEmployee.User.Person.Name, asset.CreatedOn));
                }
            }
            Response.Write(sw.ToString());
            Response.End();
            return View();
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _assetRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Asset deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
