using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Attendance.DAL.Interfaces;
using Grid.Features.Attendance.Entities;
using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Infrastructure.Extensions;
using Grid.Features.LMS.Entities;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Api.Controllers
{
    public class CalendarController : GridApiBaseController
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILeaveRepository _leaveRepository;

        public CalendarController(IHolidayRepository holidayRepository,
                                    ITimeSheetRepository timeSheetRepository,
                                    ITaskRepository taskRepository,
                                    IAttendanceRepository attendanceRepository,
                                    ILeaveRepository leaveRepository)
        {
            _holidayRepository = holidayRepository;
            _timeSheetRepository = timeSheetRepository;
            _taskRepository = taskRepository;
            _attendanceRepository = attendanceRepository;
            _leaveRepository = leaveRepository;
        }

        public ActionResult TimeSheets(EventDataFilter vm)
        {
            Func<IQueryable<TimeSheet>, IQueryable<TimeSheet>> timeSheetFilter = q =>
            {
                q = q.Where(t => t.CreatedByUserId == WebUser.Id);

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.Date >= vm.start.Value);
                }

                if (vm.end.HasValue)
                {
                    q = q.Where(t => t.Date <= vm.end.Value);
                }

                return q;
            };

            var timeSheets = _timeSheetRepository.Search(timeSheetFilter);

            var payload = timeSheets.Select(h => new EventData
            {
                id = h.Id,
                allDay = true,
                title = $"{h.Date.ToShortDateString()} - {h.TotalHours} hours - {h.State}",
                start = h.Date.ToString("s"),
                end = h.Date.ToString("s")
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Tasks(EventDataFilter vm)
        {
            Func<IQueryable<Task>, IQueryable<Task>> taskFilter = q =>
            {
                q = q.Where(t => t.AssigneeId == WebUser.Id);

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.DueDate >= vm.start.Value);
                }

                if (vm.end.HasValue)
                {
                    q = q.Where(t => t.DueDate <= vm.end.Value);
                }

                return q;
            };

            var tasks = _taskRepository.Search(taskFilter);

            var payload = tasks.Select(h => new EventData
            {
                id = h.Id,
                allDay = false,
                title = h.Title,
                start = h.DueDate.GetValueOrDefault().ToString("s"),
                end = h.DueDate?.AddHours(h.ExpectedTime.GetValueOrDefault()).ToString("s") ?? ""
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Holidays(EventDataFilter vm)
        {
            Func<IQueryable<Holiday>, IQueryable<Holiday>> holidayFilter = q =>
            {
                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.Date >= vm.start.Value);
                }

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.Date <= vm.end.Value);
                }

                return q;
            };

            var holidays = _holidayRepository.Search(holidayFilter);

            var payload = holidays.Select(h => new EventData
            {
                id = h.Id,
                allDay = true,
                title = h.Title + "-" + Enum.GetName(typeof(HolidayType), h.Type) + "-Holiday",
                start = h.Date.ToString("s"),
                end = h.Date.ToString("s")
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Attendance(EventDataFilter vm)
        {
            Func<IQueryable<Attendance>, IQueryable<Attendance>> attendanceFilter = q =>
            {
                q = q.Where(t => t.EmployeeId == WebUser.Id);

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.LogDate >= vm.start.Value);
                }

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.LogDate <= vm.end.Value);
                }

                return q;
            };

            var attendances = _attendanceRepository.Search(attendanceFilter);

            var payload = attendances.Select(h => new EventData
            {
                id = h.Id,
                allDay = true,
                title = $"In: {new DateTime(h.InTime.Ticks).ToLocalDateTime():t} & Out: {new DateTime(h.OutTime.Ticks).ToLocalDateTime():t}",
                start = h.LogDate.ToString("s"),
                end = h.LogDate.ToString("s")
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Leaves(EventDataFilter vm)
        {
            Func<IQueryable<Leave>, IQueryable<Leave>> leaveFilter = q =>
            {
                q = q.Where(t => t.CreatedByUserId == WebUser.Id);

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.Start >= vm.start.Value);
                }

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.End <= vm.end.Value);
                }

                return q;
            };

            var leaves = _leaveRepository.Search(leaveFilter);

            var payload = leaves.Select(h => new EventData
            {
                id = h.Id,
                title = $"Leave - {h.Status}",
                start = h.Start.ToLocalDateTime().ToString("s"),
                end = h.End.ToLocalDateTime().ToString("s")
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
    }
}
