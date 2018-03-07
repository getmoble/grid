using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Api.Controllers
{
    public class RoundsController : GridApiBaseController
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoundsController(IRoundRepository roundRepository,
                                   IUnitOfWork unitOfWork)
        {
            _roundRepository = roundRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _roundRepository.GetAll(), "Interview Rounds Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _roundRepository.Get(id), "Interview Round fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Round round)
        {
            ApiResult<Round> apiResult;

            if (ModelState.IsValid)
            {
                if (round.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _roundRepository.Update(round);
                        _unitOfWork.Commit();
                        return round;
                    }, "Interview Round updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _roundRepository.Create(round);
                        _unitOfWork.Commit();
                        return round;
                    }, "Interview Round created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Round>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _roundRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Interview Round deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
