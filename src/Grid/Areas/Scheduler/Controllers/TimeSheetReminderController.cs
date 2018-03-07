using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Areas.Scheduler.Controllers
{
    public class TimeSheetReminderController : Controller
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IHolidayRepository _holidayRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;


        private readonly EmailComposerService _emailComposerService;

        public TimeSheetReminderController(ILeaveRepository leaveRepository,
                                           IHolidayRepository holidayRepository,
                                           IUserRepository userRepository,
                                           EmailComposerService emailComposerService,
                                           ITimeSheetRepository timeSheetRepository)
        {
            _leaveRepository = leaveRepository;
            _holidayRepository = holidayRepository;
            _userRepository = userRepository;
            _timeSheetRepository = timeSheetRepository;
            _emailComposerService = emailComposerService;
        }

        public ActionResult Index()
        {
            var today = DateTime.Today;
            if (today.DayOfWeek != DayOfWeek.Sunday && today.DayOfWeek != DayOfWeek.Monday)
            {
                var todaysDate = DateTime.UtcNow.Date;
                var isHoliday = _holidayRepository.Any(h => DbFunctions.TruncateTime(h.Date) == todaysDate);
                if (!isHoliday)
                {
                    var employeeIds = _userRepository.GetAllBy(u => u.RequiresTimeSheet && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.Id).ToList();

                    var yesterday = DateTime.Today.AddDays(-1);
                    var timeSheets = _timeSheetRepository.GetAllBy(t => DbFunctions.TruncateTime(t.Date) == yesterday).Select(t => t.CreatedByUserId).ToList();
                    foreach (var employeeId in employeeIds)
                    {
                        var exists = timeSheets.Contains(employeeId);
                        if (!exists)
                        {
                            var selectedEmployeeId = employeeId;
                            var isOnLeave = _leaveRepository.Any(l => l.CreatedByUserId == selectedEmployeeId &&
                                        todaysDate >= DbFunctions.TruncateTime(l.Start) &&
                                        todaysDate <= DbFunctions.TruncateTime(l.End));
                            if (!isOnLeave)
                            {
                                #if !DEBUG
                                    _emailComposerService.SendReminderEmail(selectedEmployeeId, yesterday);
                                #endif
                            }
                        }
                    }

                    // Just Send the response as true
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                // Just Send the response as true
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            // Just Send the response as true
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Missed()
        {
            var today = DateTime.Today;
            if (today.DayOfWeek != DayOfWeek.Sunday && today.DayOfWeek != DayOfWeek.Monday)
            {
                var todaysDate = DateTime.UtcNow.Date;
                var isHoliday = _holidayRepository.Any(h => DbFunctions.TruncateTime(h.Date) == todaysDate);
                if (!isHoliday)
                {
                    var employeeIds = _userRepository.GetAllBy(u => u.RequiresTimeSheet && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.Id).ToList();

                    var yesterday = DateTime.Today.AddDays(-1);
                    var timeSheets = _timeSheetRepository.GetAllBy(t => DbFunctions.TruncateTime(t.Date) == yesterday).Select(t => t.CreatedByUserId).ToList();
                    foreach (var employeeId in employeeIds)
                    {
                        var exists = timeSheets.Contains(employeeId);
                        if (!exists)
                        {
                            var selectedEmployeeId = employeeId;
                            var isOnLeave = _leaveRepository.Any(l => l.CreatedByUserId == selectedEmployeeId &&
                                        todaysDate >= DbFunctions.TruncateTime(l.Start) &&
                                        todaysDate <= DbFunctions.TruncateTime(l.End));
                            if (!isOnLeave)
                            {
#if !DEBUG
                                    _emailComposerService.SendMissedEmail(selectedEmployeeId, yesterday);
#endif
                            }
                        }
                    }

                    // Just Send the response as true
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                // Just Send the response as true
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            // Just Send the response as true
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Summary()
        {
            var today = DateTime.Today;

            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                var employeeIds = _userRepository.GetAllBy(u => u.RequiresTimeSheet && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.Id).ToList();
                foreach (var employeeId in employeeIds)
                {
                    var selectedEmployeeId = employeeId;
                    #if !DEBUG
                        _emailComposerService.SummaryEmail(selectedEmployeeId);
                    #endif
                }

                // Just Send the response as true
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            // Just Send the response as false
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovalReminder()
        {
            var today = DateTime.Today;
            if (today.DayOfWeek != DayOfWeek.Sunday && today.DayOfWeek != DayOfWeek.Monday)
            {
                var todaysDate = DateTime.UtcNow.Date;
                var isHoliday = _holidayRepository.Any(h => DbFunctions.TruncateTime(h.Date) == todaysDate);
                if (!isHoliday)
                {
                    var pendingTimeSheetApprovers = _timeSheetRepository.GetAllBy(t => t.State == TimeSheetState.PendingApproval && t.CreatedByUser.ReportingPersonId.HasValue, "CreatedByUser").Select(t => t.CreatedByUser.ReportingPersonId).ToList().Distinct();
                    foreach (var pendingTimeSheetApprover in pendingTimeSheetApprovers)
                    {
                        #if !DEBUG
                            _emailComposerService.SendApprovalReminderEmail(pendingTimeSheetApprover.GetValueOrDefault());
                        #endif
                    }

                    // Just Send the response as true
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                // Just Send the response as true
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            // Just Send the response as true
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}