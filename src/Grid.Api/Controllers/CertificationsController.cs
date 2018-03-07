using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class CertificationsController : GridApiBaseController
    {
        private readonly ICertificationRepository _certificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CertificationsController(ICertificationRepository certificationRepository,
                                   IUnitOfWork unitOfWork)
        {
            _certificationRepository = certificationRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _certificationRepository.GetAll(), "Certifications Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _certificationRepository.Get(id), "Certification Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Certification certification)
        {
            ApiResult<Certification> apiResult;

            if (ModelState.IsValid)
            {
                if (certification.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _certificationRepository.Update(certification);
                        _unitOfWork.Commit();
                        return certification;
                    }, "Certification updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _certificationRepository.Create(certification);
                        _unitOfWork.Commit();
                        return certification;
                    }, "Certification created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Certification>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _certificationRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Certification deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
