using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.Company.Controllers
{
    [GridPermission(PermissionCode = 200)]
    public class LocationsController : CompanyBaseController
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationsController(ILocationRepository locationRepository,
                                   IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
