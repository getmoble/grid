using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;

namespace Grid.Api.Controllers
{
    public class VendorsController : GridApiBaseController
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VendorsController(IVendorRepository vendorRepository,
                                   IUnitOfWork unitOfWork)
        {
            _vendorRepository = vendorRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _vendorRepository.GetAll(), "Vendors Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _vendorRepository.Get(id), "Holiday fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Vendor vendor)
        {
            ApiResult<Vendor> apiResult;

            if (ModelState.IsValid)
            {
                if (vendor.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {

                        var newVendor = new Vendor
                        {
                           
                            Title = vendor.Title,
                            Email = vendor.Email,
                            Phone = vendor.Phone,
                            WebSite = vendor.WebSite,
                            Address = vendor.Address,
                            ContactPerson = vendor.ContactPerson,
                            ContactPersonEmail = vendor.ContactPersonEmail,
                            ContactPersonPhone = vendor.ContactPersonPhone,
                            Description = vendor.Description,                                                  
                        };
                        _vendorRepository.Update(vendor);
                        _unitOfWork.Commit();
                        return vendor;
                    }, "Vendor updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newVendor = new Vendor
                        {
                            Id = vendor.Id,
                            Title = vendor.Title,
                            Email = vendor.Email,
                            Phone = vendor.Phone,
                            WebSite = vendor.WebSite,
                            Address = vendor.Address,
                            ContactPerson = vendor.ContactPerson,
                            ContactPersonEmail = vendor.ContactPersonEmail,
                            ContactPersonPhone = vendor.ContactPersonPhone,
                            Description = vendor.Description,
                        };
                        _vendorRepository.Create(vendor);
                        _unitOfWork.Commit();
                        return vendor;
                    }, "Vendor created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Vendor>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _vendorRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Vendor deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
