using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Grid.Areas.LMS.Models;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.ViewModels;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.LMS.Services.Interfaces;
using Grid.Filters;
using Grid.Infrastructure.Extensions;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.LMS.Controllers
{
    public class LeavesController : LeaveBaseController
    {
        private readonly ILeaveService _leaveService;

        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveEntitlementRepository _leaveEntitlementRepository;
        private readonly ILeaveEntitlementUpdateRepository _leaveEntitlementUpdateRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveTimePeriodRepository _leaveTimePeriodRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly EmailComposerService _emailComposerService;

        public LeavesController(ILeaveService leaveService,
                                ILeaveRepository leaveRepository,
                                ILeaveEntitlementRepository leaveEntitlementRepository,
                                ILeaveEntitlementUpdateRepository leaveEntitlementUpdateRepository,
                                ILeaveTypeRepository leaveTypeRepository,
                                IEmployeeRepository employeeRepository,
                                ILeaveTimePeriodRepository leaveTimePeriodRepository,
                                IUserRepository userRepository,
                                EmailComposerService emailComposerService,
                                IUnitOfWork unitOfWork)
        {
            _leaveService = leaveService;
            _leaveRepository = leaveRepository;
            _leaveEntitlementRepository = leaveEntitlementRepository;
            _leaveEntitlementUpdateRepository = leaveEntitlementUpdateRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _employeeRepository = employeeRepository;
            _leaveTimePeriodRepository = leaveTimePeriodRepository;
            _userRepository = userRepository;

            _unitOfWork = unitOfWork;

            _emailComposerService = emailComposerService;
        }


        [GridPermission(PermissionCode = 300)]
        public ActionResult MyHistory()
        {
            var leaveHistory = _leaveEntitlementUpdateRepository.GetAllBy(u => u.EmployeeId == WebUser.Id, "LeaveTimePeriod,LeaveType,CreatedByUser.Person").OrderByDescending(u => u.CreatedOn).ToList();
            return View(leaveHistory);
        }
        
        public ActionResult Manage()
        {
            return View();
        }
        public ActionResult Index() {
            return View();
        }
        public ActionResult MyLeaveBalance()
        {
            var employeeId = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User");
            ViewBag.employeeId = employeeId.Id;
            return View();
        }
        public ActionResult EmployeesLeaveBalance()
        {
            var employeeId = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User");
            ViewBag.employeeId = employeeId.Id;
            return View();
        }
        
    }
}
