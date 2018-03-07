using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Api.Controllers
{
    public class InterviewsController : GridApiBaseController
    {
        private readonly IInterviewRoundRepository _interviewRoundRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InterviewsController(IInterviewRoundRepository interviewRoundRepository,
                                   IUnitOfWork unitOfWork)
        {
            _interviewRoundRepository = interviewRoundRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _interviewRoundRepository.GetAll(), "Interviews Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _interviewRoundRepository.Get(id), "Interview Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(InterviewRound referal)
        {
            ApiResult<InterviewRound> apiResult;

            if (ModelState.IsValid)
            {
                if (referal.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _interviewRoundRepository.Update(referal);
                        _unitOfWork.Commit();
                        return referal;
                    }, "Interview updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _interviewRoundRepository.Create(referal);
                        _unitOfWork.Commit();
                        return referal;
                    }, "Interview created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<InterviewRound>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _interviewRoundRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Interview deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
