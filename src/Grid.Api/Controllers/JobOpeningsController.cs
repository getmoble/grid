using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.Recruit;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Api.Controllers
{
    public class JobOpeningsController : GridApiBaseController
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JobOpeningsController(IJobOpeningRepository jobOpeningRepository,
                                   IUnitOfWork unitOfWork)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                var jobOpenings = _jobOpeningRepository.GetAll();
                return jobOpenings.Select(j => new JobOpeningModel(j)).ToList();
            }, "Job Openings Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _jobOpeningRepository.Get(id), "Job Opening Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(JobOpening jobOpening)
        {
            ApiResult<JobOpening> apiResult;

            if (ModelState.IsValid)
            {
                if (jobOpening.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _jobOpeningRepository.Update(jobOpening);
                        _unitOfWork.Commit();
                        return jobOpening;
                    }, "Job Opening updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _jobOpeningRepository.Create(jobOpening);
                        _unitOfWork.Commit();
                        return jobOpening;
                    }, "Job Opening created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<JobOpening>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _jobOpeningRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Job Opening deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
