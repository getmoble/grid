using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.LMS.Controllers
{
    [GridPermission(PermissionCode = 215)]
    public class LeavePeriodsController : LeaveBaseController
    {
        private readonly ILeaveTimePeriodRepository _leaveTimePeriodRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeavePeriodsController(ILeaveTimePeriodRepository leaveTimePeriodRepository,
                                          IUnitOfWork unitOfWork)
        {
            _leaveTimePeriodRepository = leaveTimePeriodRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var leavePeriods = _leaveTimePeriodRepository.GetAll();
            return View(leavePeriods);
        }
    }
}
