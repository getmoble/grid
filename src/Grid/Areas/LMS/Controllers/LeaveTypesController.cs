using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.LMS.Controllers
{
    [GridPermission(PermissionCode = 215)]
    public class LeaveTypesController : LeaveBaseController
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypesController(ILeaveTypeRepository leaveTypeRepository,
                                    IUnitOfWork unitOfWork)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var leaveTypes = _leaveTypeRepository.GetAll();
            return View(leaveTypes);
        }       
    }
}
