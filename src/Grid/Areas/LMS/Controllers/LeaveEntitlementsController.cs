using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;
using Grid.Features.LMS.ViewModels;
using Grid.Features.LMS.Entities.Enums;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.LMS.Controllers
{
    [GridPermission(PermissionCode = 215)]
    public class LeaveEntitlementsController : LeaveBaseController
    {
        private readonly ILeaveEntitlementRepository _leaveEntitlementRepository;
        private readonly ILeaveEntitlementUpdateRepository _leaveEntitlementUpdateRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveTimePeriodRepository _leaveTimePeriodRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveEntitlementsController(ILeaveEntitlementRepository leaveEntitlementRepository,
                                           ILeaveEntitlementUpdateRepository leaveEntitlementUpdateRepository,
                                           ILeaveTimePeriodRepository leaveTimePeriodRepository,
                                           ILeaveTypeRepository leaveTypeRepository,
                                           IUserRepository userRepository,
                                           IUnitOfWork unitOfWork)
        {
            _leaveEntitlementRepository = leaveEntitlementRepository;
            _leaveEntitlementUpdateRepository = leaveEntitlementUpdateRepository;
            _leaveTimePeriodRepository = leaveTimePeriodRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }   
        public ActionResult Index()
        {
            var leaveEntitlements = _leaveEntitlementRepository.GetAll();
            return View(leaveEntitlements);
        }                        
    }
}
