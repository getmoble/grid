using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;

namespace Grid.Areas.Company.Controllers
{
    public class AwardsController : GridBaseController
    {
        private readonly IAwardRepository _awardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AwardsController(IAwardRepository awardRepository,
                                IUnitOfWork unitOfWork)
        {
            _awardRepository = awardRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var awards = _awardRepository.GetAll();
            return View(awards);
        }

    }
}
