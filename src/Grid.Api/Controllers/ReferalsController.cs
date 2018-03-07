using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Api.Controllers
{
    public class ReferalsController : GridApiBaseController
    {
        private readonly IReferalRepository _referalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReferalsController(IReferalRepository referalRepository,
                                   IUnitOfWork unitOfWork)
        {
            _referalRepository = referalRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _referalRepository.GetAll(), "Referals Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _referalRepository.Get(id), "Referal Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Referal referal)
        {
            ApiResult<Referal> apiResult;

            if (ModelState.IsValid)
            {
                if (referal.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _referalRepository.Update(referal);
                        _unitOfWork.Commit();
                        return referal;
                    }, "Referal updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _referalRepository.Create(referal);
                        _unitOfWork.Commit();
                        return referal;
                    }, "Referal created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Referal>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _referalRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Referal deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
