using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Api.Controllers
{
    public class PotentialsController : GridApiBaseController
    {
        private readonly ICRMPotentialRepository _crmPotentialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PotentialsController(ICRMPotentialRepository crmPotentialRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmPotentialRepository = crmPotentialRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmPotentialRepository.GetAll(), "Potentials Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _crmPotentialRepository.Get(id), "Potential fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMPotential potential)
        {
            ApiResult<CRMPotential> apiResult;

            if (ModelState.IsValid)
            {
                if (potential.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _crmPotentialRepository.Update(potential);
                        _unitOfWork.Commit();
                        return potential;
                    }, "Potential updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _crmPotentialRepository.Create(potential);
                        _unitOfWork.Commit();
                        return potential;
                    }, "Potential created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMPotential>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmPotentialRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Potential deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
