using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Api.Controllers
{
    public class CandidateDesignationsController : GridApiBaseController
    {
        private readonly ICandidateDesignationRepository _candidateDesignationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CandidateDesignationsController(ICandidateDesignationRepository candidateDesignationRepository,
                                   IUnitOfWork unitOfWork)
        {
            _candidateDesignationRepository = candidateDesignationRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _candidateDesignationRepository.GetAll(), "Candidate Designations Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _candidateDesignationRepository.Get(id), "Candidate Designation fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CandidateDesignation candidateDesignation)
        {
            ApiResult<CandidateDesignation> apiResult;

            if (ModelState.IsValid)
            {
                if (candidateDesignation.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _candidateDesignationRepository.Update(candidateDesignation);
                        _unitOfWork.Commit();
                        return candidateDesignation;
                    }, "Candidate Designation updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _candidateDesignationRepository.Create(candidateDesignation);
                        _unitOfWork.Commit();
                        return candidateDesignation;
                    }, "Candidate Designation created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CandidateDesignation>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _candidateDesignationRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Candidate Designation deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
