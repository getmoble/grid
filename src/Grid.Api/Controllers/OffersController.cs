using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Api.Controllers
{
    public class OffersController : GridApiBaseController
    {
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OffersController(IJobOfferRepository jobOfferRepository,
                                   IUnitOfWork unitOfWork)
        {
            _jobOfferRepository = jobOfferRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _jobOfferRepository.GetAll(), "Job Offers Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _jobOfferRepository.Get(id), "JobOffer fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(JobOffer round)
        {
            ApiResult<JobOffer> apiResult;

            if (ModelState.IsValid)
            {
                if (round.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _jobOfferRepository.Update(round);
                        _unitOfWork.Commit();
                        return round;
                    }, "Job Offer updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _jobOfferRepository.Create(round);
                        _unitOfWork.Commit();
                        return round;
                    }, "Job Offer created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<JobOffer>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _jobOfferRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Job Offer deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
