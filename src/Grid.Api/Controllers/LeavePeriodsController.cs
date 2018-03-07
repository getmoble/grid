using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Api.Controllers
{
    public class LeavePeriodsController : GridApiBaseController
    {
        private readonly ILeaveTimePeriodRepository _leaveTimePeriodRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeavePeriodsController(ILeaveTimePeriodRepository leaveTimePeriodRepository,
                                   IUnitOfWork unitOfWork)
        {
            _leaveTimePeriodRepository = leaveTimePeriodRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _leaveTimePeriodRepository.GetAll(), "Leave Periods Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _leaveTimePeriodRepository.Get(id), "Leave Time Period fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(LeaveTimePeriod holiday)
        {
            ApiResult<LeaveTimePeriod> apiResult;

            if (ModelState.IsValid)
            {
                if (holiday.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _leaveTimePeriodRepository.Update(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Leave Time Period updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _leaveTimePeriodRepository.Create(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Leave Time Period created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<LeaveTimePeriod>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _leaveTimePeriodRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Leave Period deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
