using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class ShiftsController : GridBaseController
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShiftsController(IShiftRepository shiftRepository,
                                IUnitOfWork unitOfWork)
        {
            _shiftRepository = shiftRepository;
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            var shifts = _shiftRepository.GetAll();
            return View(shifts);
        }

    }
}
