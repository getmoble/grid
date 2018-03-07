using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Api.Controllers
{
    public class LeaveTypesController : GridApiBaseController
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypesController(ILeaveTypeRepository leaveTypeRepository,
                                   IUnitOfWork unitOfWork)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _leaveTypeRepository.GetAll(), "Leave Types Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _leaveTypeRepository.Get(id), "Leave Type fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(LeaveType holiday)
        {
            ApiResult<LeaveType> apiResult;

            if (ModelState.IsValid)
            {
                if (holiday.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _leaveTypeRepository.Update(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Leave Type updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _leaveTypeRepository.Create(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Leave Type created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<LeaveType>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _leaveTypeRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Leave Type deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
