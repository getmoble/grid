using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.LMS;
using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Api.Controllers
{
    public class HolidayController : GridApiBaseController
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HolidayController(IHolidayRepository holidayRepository,
                                   IUnitOfWork unitOfWork)
        {
            _holidayRepository = holidayRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _holidayRepository.GetAll().Select(h => new HolidayModel(h)).ToList();
            }, "Holidays Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _holidayRepository.Get(id), "Holiday fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Holiday holiday)
        {
            ApiResult<Holiday> apiResult;

            if (ModelState.IsValid)
            {
                if (holiday.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _holidayRepository.Update(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Holiday updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _holidayRepository.Create(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Holiday created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Holiday>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _holidayRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Holiday deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
