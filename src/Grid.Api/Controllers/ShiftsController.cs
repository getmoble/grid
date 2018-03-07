using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class ShiftsController : GridApiBaseController
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShiftsController(IShiftRepository shiftRepository,
                                   IUnitOfWork unitOfWork)
        {
            _shiftRepository = shiftRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _shiftRepository.GetAll(), "Shifts Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _shiftRepository.Get(id), "Shift Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Shift shift)
        {
            ApiResult<Shift> apiResult;

            if (ModelState.IsValid)
            {
                if (shift.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _shiftRepository.Update(shift);
                        _unitOfWork.Commit();
                        return shift;
                    }, "Shift updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _shiftRepository.Create(shift);
                        _unitOfWork.Commit();
                        return shift;
                    }, "Shift created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Shift>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _shiftRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Shift deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
