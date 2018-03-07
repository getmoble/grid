using System.Web.Mvc;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities.Enums;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Api.Controllers
{
    [APIIdentityInjector]
    public class JobOpeningController : GridBaseController
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;

        public JobOpeningController(IJobOpeningRepository jobOpeningRepository)
        {
            _jobOpeningRepository = jobOpeningRepository;
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
            var openings = _jobOpeningRepository.GetAllBy(j => j.OpeningStatus == JobOpeningStatus.Open);
            return Json(openings, JsonRequestBehavior.AllowGet);
        }
    }
}