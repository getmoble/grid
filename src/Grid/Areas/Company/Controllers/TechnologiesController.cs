using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Infrastructure;

namespace Grid.Areas.Company.Controllers
{
    public class TechnologiesController : GridBaseController
    {
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TechnologiesController(ITechnologyRepository technologyRepository,
                                   IUnitOfWork unitOfWork)
        {
            _technologyRepository = technologyRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var technologies = _technologyRepository.GetAll();
            return View(technologies);
        }
    }
}
