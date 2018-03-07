using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;
using System.Linq;
using Grid.Api.Models.IT;

namespace Grid.Api.Controllers
{
    public class SoftwaresController : GridApiBaseController
    {
        private readonly ISoftwareRepository _softwareRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SoftwaresController(ISoftwareRepository softwareRepository,
                                   IUnitOfWork unitOfWork)
        {
            _softwareRepository = softwareRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {           
            var apiResult = TryExecute(() =>
            {
                return _softwareRepository.GetAll().Select(h => new SoftwareModel(h)).ToList();
            }, "Softwares Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);       
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() => _softwareRepository.Get(id, "Category"), "Software fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(SoftwareModel softwareModel)
        {
            ApiResult<Software> apiResult;

            if (ModelState.IsValid)
            {
                if (softwareModel.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var software = new Software
                        {
                            Title = softwareModel.Title,
                            Version = softwareModel.Version,
                            Description = softwareModel.Description,
                            LatestVersion = softwareModel.LatestVersion,
                            RecommendedVersion = softwareModel.RecommendedVersion,
                            Status = softwareModel.Status,
                            LicenseType = softwareModel.LicenseType,                           
                            SoftwareCategoryId = softwareModel.SoftwareCategoryId,
                            Id = softwareModel.Id
                        };
                        _softwareRepository.Update(software);
                        _unitOfWork.Commit();
                        return software;
                    }, "Software updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var software = new Software
                        {
                            Title = softwareModel.Title,
                            Version = softwareModel.Version,
                            Description = softwareModel.Description,
                            LatestVersion = softwareModel.LatestVersion,
                            RecommendedVersion = softwareModel.RecommendedVersion,
                            Status = softwareModel.Status,
                            LicenseType = softwareModel.LicenseType,
                            LicensesAllowed = softwareModel.LicensesAllowed,
                            SoftwareCategoryId = softwareModel.SoftwareCategoryId,
                            Id = softwareModel.Id
                        };
                        _softwareRepository.Create(software);
                        _unitOfWork.Commit();
                        return software;
                    }, "Software created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Software>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _softwareRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Software deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
