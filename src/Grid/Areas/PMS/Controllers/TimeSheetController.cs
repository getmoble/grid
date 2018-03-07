using FluentDateTime;
using Grid.Areas.PMS.Models;
using Grid.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Hubs;
using Grid.Infrastructure.Extensions;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.PMS.ViewModels;
using Grid.Infrastructure.Filters;
using Grid.Infrastructure;

namespace Grid.Areas.PMS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class TimeSheetController : ProjectsBaseController
    {
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;
        private readonly ITimeSheetActivityRepository _timeSheetActivityRepository;
        private readonly IMissedTimeSheetRepository _missedTimeSheetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly INotificationService _notificationService;
        private readonly EmailComposerService _emailComposerService;
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _unitOfWork;

        public TimeSheetController(ITimeSheetRepository timeSheetRepository,
                                   ITimeSheetLineItemRepository timeSheetLineItemRepository,
                                   ITimeSheetActivityRepository timeSheetActivityRepository,
                                   IMissedTimeSheetRepository missedTimeSheetRepository,
                                   IUserRepository userRepository,
                                   IProjectRepository projectRepository,
                                   IProjectMemberRepository projectMemberRepository,
                                   INotificationService notificationService,
                                   EmailComposerService emailComposerService,
                                   IEmployeeRepository employeeRepository,
                                   IUnitOfWork unitOfWork)
        {
            _timeSheetRepository = timeSheetRepository;
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
            _timeSheetActivityRepository = timeSheetActivityRepository;
            _missedTimeSheetRepository = missedTimeSheetRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _projectMemberRepository = projectMemberRepository;
            _notificationService = notificationService;
            _emailComposerService = emailComposerService;

            _unitOfWork = unitOfWork;
        }

        #region AjaxCalls
        [HttpGet]
        public FileContentResult EffortByProjectCSV(TimeSheetSearchViewModel vm)
        {
            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetLineItemFilter = q =>
            {
                q = q.Include("TimeSheet").Include("Project");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.CreatedByUserId == vm.SubmittedUserById.Value);
                }

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }

                if (vm.TeamMode)
                {
                    var teamMembers = new List<int>();

                    if (WebUser.IsAdmin || PermissionChecker.CheckPermission(WebUser.Permissions, 210))
                    {
                        teamMembers.AddRange(_userRepository.GetAll().Select(u => u.Id).ToList());
                    }
                    else
                    {
                        var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                        var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.UserId).ToList();
                        var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id) || u.Id == WebUser.Id, "Person").Select(u => u.Id).ToList();
                        teamMembers.AddRange(userList);
                    }
                   
                    q = q.Where(r => teamMembers.Contains(r.TimeSheet.CreatedByUserId));
                }

                return q;
            };

            var csv = new StringBuilder();
            var employeesByLocation = _timeSheetLineItemRepository.Search(timeSheetLineItemFilter).GroupBy(l => l.Project.Title)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               Value = x.Sum(i => i.Effort)
                                           })
                                            .ToList();

            var keys = string.Join(",", employeesByLocation.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", employeesByLocation.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EmployeesByLocationCSV.csv");
        }

        [HttpGet]
        public FileContentResult EffortByBillableCSV(TimeSheetSearchViewModel vm)
        {
            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetLineItemFilter = q =>
            {
                q = q.Include("TimeSheet.CreatedByUser");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.CreatedByUserId == vm.SubmittedUserById.Value);
                }

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }

                if (vm.TeamMode)
                {
                    var teamMembers = new List<int>();
                    if (WebUser.IsAdmin || PermissionChecker.CheckPermission(WebUser.Permissions, 210))
                    {
                        teamMembers.AddRange(_userRepository.GetAll().Select(u => u.Id).ToList());
                    }
                    else
                    {
                        var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                        var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id).Select(u => u.UserId).ToList();
                        var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id) || u.Id == WebUser.Id, "Person").Select(u => u.Id).ToList();
                        teamMembers.AddRange(userList);
                    }
                    q = q.Where(r => teamMembers.Contains(r.TimeSheet.CreatedByUserId));
                }

                return q;
            };

            var csv = new StringBuilder();
            var employeesByLocation = _timeSheetLineItemRepository.Search(timeSheetLineItemFilter).GroupBy(l => l.WorkType)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               workType = (x.Key == 1 ? "Billable" : "Non-Billable"),
                                               Value = x.Sum(i => i.Effort)
                                           })
                                            .ToList();

            var keys = string.Join(",", employeesByLocation.Select(x => x.workType).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", employeesByLocation.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EffortByBillableCSV.csv");
        }

        [HttpGet]
        public FileContentResult EffortByBillableProjectsCSV(TimeSheetSearchViewModel vm)
        {
            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetLineItemFilter = q =>
            {
                q = q.Include("TimeSheet.CreatedByUser");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.CreatedByUserId == vm.SubmittedUserById.Value);
                }

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }
                if (vm.ProjectId.HasValue)
                {
                    q = q.Where(r => r.ProjectId == vm.ProjectId);
                }

                if (vm.TeamMode)
                {
                    var teamMembers = new List<int>();
                    if (WebUser.IsAdmin || PermissionChecker.CheckPermission(WebUser.Permissions, 210))
                    {
                        teamMembers.AddRange(_userRepository.GetAll().Select(u => u.Id).ToList());
                    }
                    else
                    {
                        var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                        var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id).Select(u => u.UserId).ToList();
                        var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id) || u.Id == WebUser.Id, "Person").Select(u => u.Id).ToList();
                        teamMembers.AddRange(userList);
                    }
                    q = q.Where(r => teamMembers.Contains(r.TimeSheet.CreatedByUserId));
                }

                return q;
            };

            var csv = new StringBuilder();
            var billableByProjects = _timeSheetLineItemRepository.Search(timeSheetLineItemFilter).GroupBy(l => l.WorkType)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               workType = (x.Key == 1 ? "Billable" : "Non-Billable"),
                                               Value = x.Sum(i => i.Effort)
                                           })
                                            .ToList();


            var keys = string.Join(",", billableByProjects.Select(x => x.workType).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", billableByProjects.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "BillableByProjectTypeCSV.csv");

            // Bad, we are getting numbers. So I put the name instead
            //csv.AppendLine("Billable,Non-Billable");
            //csv.AppendLine(  "Billable,Non-Billable");
            //var values = string.Join(",", billableByProjects.Select(x => x.Value).ToArray());
            //csv.AppendLine(values);
        }


        [HttpGet]
        public ActionResult GetAllProjectsForTimeSheet()
        {
            // Get only Projects to which i am added            
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person");
            var myProjects = _projectMemberRepository.GetAllBy(m => m.EmployeeId == employee.Id && m.MemberStatus == MemberStatus.Active, "Project").Select(m => m.Project).ToList();
            var getOnlyOpenProjects = myProjects.Where(p => !(p.Status == ProjectStatus.Cancelled || p.Status == ProjectStatus.Closed)).ToList();

            // Get all public Projects
            var publicProjects = _projectRepository.GetAllBy(p => p.IsPublic && !(p.Status == ProjectStatus.Cancelled || p.Status == ProjectStatus.Closed)).ToList();

            // Merge public to this list
            foreach (var pp in publicProjects)
            {
                if (getOnlyOpenProjects.All(p => p.Id != pp.Id))
                {
                    getOnlyOpenProjects.Add(pp);
                }
            }

            return Json(getOnlyOpenProjects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllProjectsWorkType(long id)
        {
            // Get project workType 
            Billing workType;
            var getProject = _projectRepository.GetBy(u => u.Id == id);
            if (getProject.IsPublic == true)
            {
                workType = Billing.NonBillable;
            }
            else
            {
                var getProjectMember = _projectMemberRepository.GetBy(u => u.ProjectId == id);
                workType = getProjectMember.Billing;
            }
            return Json(workType.ToString(), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult CreateSheet(TimeSheetModel timeSheet)
        {
            var date = timeSheet.Date.DateFromUsFormat();
            var exists = _timeSheetRepository.Any(t => t.CreatedByUserId == WebUser.Id && t.Date == date.Date);
            if (!exists)
            {
                var newTimeSheet = new TimeSheet
                {
                    State = TimeSheetState.PendingApproval,
                    Title = $"{WebUser.Name}'s {"TimeSheet for "} {timeSheet.Date}",
                    Date = date.Date,
                    TotalHours = timeSheet.Rows.Sum(r => r.Effort),
                    Comments = timeSheet.Comments,
                    CreatedByUserId = WebUser.Id
                };

                _timeSheetRepository.Create(newTimeSheet);

                foreach (var lineItem in timeSheet.Rows)
                {
                    var newTimeSheetLineItem = new TimeSheetLineItem
                    {
                        TimeSheetId = newTimeSheet.Id,
                        ProjectId = lineItem.ProjectId,
                        TaskId = lineItem.TaskId,
                        TaskSummary = lineItem.TaskSummary,
                        Effort = lineItem.Effort,
                        Comments = lineItem.Comments,
                        WorkType = lineItem.WorkType
                    };

                    _timeSheetLineItemRepository.Create(newTimeSheetLineItem);
                }

                _unitOfWork.Commit();

                // Log Activity 
                var activity = new TimeSheetActivity
                {
                    TimeSheetId = newTimeSheet.Id,
                    Title = "Created",
                    Comment = $"{WebUser.Name} created the TimeSheet at {DateTime.UtcNow.ToString("G")} with state {newTimeSheet.State} & hours {newTimeSheet.TotalHours}",
                    CreatedByUserId = WebUser.Id
                };

                _timeSheetActivityRepository.Create(activity);
                _unitOfWork.Commit();

                //Update if there is a missing timesheet entry
                var missedList = _missedTimeSheetRepository.GetAllBy(m => m.UserId == newTimeSheet.CreatedByUserId && m.Date == newTimeSheet.Date && m.FilledOn == null);
                foreach (var missedTimeSheet in missedList)
                {
                    missedTimeSheet.FilledOn = DateTime.UtcNow;
                    _missedTimeSheetRepository.Update(missedTimeSheet);
                }

                _unitOfWork.Commit();

                // Notify the reporting manager online
                var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id, "ReportingPerson");
                if (loginEmployee?.ReportingPerson != null)
                {
                    var message = $"{WebUser.Name} has submitted the timesheet with {newTimeSheet.TotalHours} hours";
                    _notificationService.NotifyUser("Timesheet Submitted", message, loginEmployee.ReportingPerson.Code);
                }

                // Send Email, Email Template name is hard corded - Need to change later
                // Replace the hard coded emails with settings or a team.
#if !DEBUG
                    _emailComposerService.TimeSheetSubmitted(newTimeSheet.Id);
#endif

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public ActionResult UpdateSheet(TimeSheetModel timeSheet)
        {
            var date = timeSheet.Date.DateFromUsFormat();

            // Check for duplicates
            var exists = _timeSheetRepository.Any(t => t.CreatedByUserId == WebUser.Id && t.Date == date.Date && t.Id != timeSheet.Id);
            if (exists)
            {
                return Json(false);
            }

            var selectedSheet = _timeSheetRepository.Get(timeSheet.Id);
            if (selectedSheet != null)
            {
                selectedSheet.Title = $"{WebUser.Name}'s {"TimeSheet for "} {timeSheet.Date}";
                selectedSheet.Date = date.Date;
                selectedSheet.State = TimeSheetState.PendingApproval;
                selectedSheet.TotalHours = timeSheet.Rows.Sum(r => r.Effort);
                selectedSheet.Comments = timeSheet.Comments;

                selectedSheet.UpdatedByUserId = WebUser.Id;

                _timeSheetRepository.Update(selectedSheet);
                _unitOfWork.Commit();

                // Remove Existing
                var existingRows = _timeSheetLineItemRepository.GetAllBy(t => t.TimeSheetId == timeSheet.Id).ToList();
                foreach (var existingRow in existingRows)
                {
                    _timeSheetLineItemRepository.Delete(existingRow);
                }

                _unitOfWork.Commit();

                // Add Fresh
                foreach (var lineItem in timeSheet.Rows)
                {
                    var newTimeSheetLineItem = new TimeSheetLineItem
                    {
                        TimeSheetId = selectedSheet.Id,
                        ProjectId = lineItem.ProjectId,
                        TaskId = lineItem.TaskId,
                        TaskSummary = lineItem.TaskSummary,
                        Effort = lineItem.Effort,
                        Comments = lineItem.Comments,
                        WorkType = lineItem.WorkType
                    };

                    _timeSheetLineItemRepository.Create(newTimeSheetLineItem);
                }

                _unitOfWork.Commit();

                // Log Activity 
                var activity = new TimeSheetActivity
                {
                    TimeSheetId = selectedSheet.Id,
                    Title = "Updated",
                    Comment = $"{WebUser.Name} updated the TimeSheet at {DateTime.UtcNow.ToString("G")} with state {selectedSheet.State} & hours {selectedSheet.TotalHours}",
                    CreatedByUserId = WebUser.Id
                };

                _timeSheetActivityRepository.Create(activity);
                _unitOfWork.Commit();

                // Notify the reporting manager online
                var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id, "ReportingPerson");
                if (loginEmployee?.ReportingPerson != null)
                {
                    var message = $"{WebUser.Name} has updated the timesheet with {selectedSheet.TotalHours} hours";
                    _notificationService.NotifyUser("Timesheet Updated", message, loginEmployee.ReportingPerson.Code);
                }

                // Send Email, Email Template name is hard corded - Need to change later
                // Replace the hard coded emails with settings or a team.
#if !DEBUG
                    _emailComposerService.TimeSheetUpdated(selectedSheet.Id);
#endif

                return Json(true);
            }

            return Json(false);
        }

        [HttpGet]
        public ActionResult GetAllSheets(EventDataFilter vm)
        {
            Func<IQueryable<TimeSheet>, IQueryable<TimeSheet>> timeSheetFilter = q =>
            {
                q = q.Where(t => t.CreatedByUserId == WebUser.Id);

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

            var payload = _timeSheetRepository.Search(timeSheetFilter).ToList().Select(h => new TimeSheetCalenderEvent
            {
                id = h.Id,
                allDay = true,
                title = $"{h.Date.ToShortDateString()} - {h.TotalHours} hours - {h.State}",
                description = $"{h.Title} - {h.TotalHours} hours - {h.State}",
                start = h.Date.ToString("s"),
                end = h.Date.ToString("s"),
                state = h.State
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTimeSheet(int id)
        {
            var timeSheet = _timeSheetRepository.Get(id);
            var lineItems = _timeSheetLineItemRepository.GetAllBy(l => l.TimeSheetId == timeSheet.Id).ToList();
            if (timeSheet != null)
            {
                var model = new TimeSheetModel
                {
                    Id = timeSheet.Id,
                    Title = timeSheet.Title,
                    Date = timeSheet.Date.ToUsFormat(),
                    State = timeSheet.State,
                    Comments = timeSheet.Comments
                };

                var lineItemModels = lineItems.Select(l => new TimeSheetLineItemModel
                {
                    Comments = l.Comments,
                    Effort = l.Effort,
                    ProjectId = l.ProjectId,
                    TaskId = l.TaskId,
                    TaskSummary = l.TaskSummary,
                    WorkType = l.WorkType
                }).ToList();

                model.Rows = lineItemModels;

                return Json(model, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveTimeSheet(TimeSheetStatusViewModel vm)
        {
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person");
            var selectedTimeSheet = _timeSheetRepository.Get(vm.Id);
            if (selectedTimeSheet != null)
            {
                selectedTimeSheet.State = TimeSheetState.Approved;
                selectedTimeSheet.ApprovedByEmployeeId = employee.Id;
                selectedTimeSheet.ApproverComments = vm.Comments;

                _timeSheetRepository.Update(selectedTimeSheet);
                _unitOfWork.Commit();

                // Log Activity 
                var activity = new TimeSheetActivity
                {
                    TimeSheetId = selectedTimeSheet.Id,
                    Title = "Approved",
                    Comment = $"{WebUser.Name} approved the TimeSheet at {DateTime.UtcNow.ToString("G")} with comments {vm.Comments}",
                    CreatedByUserId = WebUser.Id
                };

                _timeSheetActivityRepository.Create(activity);
                _unitOfWork.Commit();


                // Notify the user online
                var submittedUser = _userRepository.Get(selectedTimeSheet.CreatedByUserId);
                if (submittedUser != null)
                {
                    var message = $"Your timesheet for {selectedTimeSheet.Date.ToShortDateString()} has been approved";
                    _notificationService.NotifyUser("Timesheet Approved", message, submittedUser.Code);
                }

                // Send Email, Email Template name is hard corded - Need to change later
                // Replace the hard coded emails with settings or a team.
#if !DEBUG
                    _emailComposerService.TimeSheetApproved(selectedTimeSheet.Id);
#endif
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult NeedsCorrection(TimeSheetStatusViewModel vm)
        {
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person");
            var selectedTimeSheet = _timeSheetRepository.Get(vm.Id);
            if (selectedTimeSheet != null)
            {
                selectedTimeSheet.State = TimeSheetState.NeedsCorrection;
                selectedTimeSheet.ApprovedByEmployeeId = employee.Id;
                selectedTimeSheet.ApproverComments = vm.Comments;

                _timeSheetRepository.Update(selectedTimeSheet);
                _unitOfWork.Commit();

                // Log Activity 
                var activity = new TimeSheetActivity
                {
                    TimeSheetId = selectedTimeSheet.Id,
                    Title = "Needs Correction",
                    Comment = $"{WebUser.Name} rejected the TimeSheet at {DateTime.UtcNow.ToString("G")} with comments {vm.Comments}",
                    CreatedByUserId = WebUser.Id
                };

                _timeSheetActivityRepository.Create(activity);
                _unitOfWork.Commit();

                // Notify the user online
                var submittedUser = _userRepository.Get(selectedTimeSheet.CreatedByUserId);
                if (submittedUser != null)
                {
                    var message = $"Your timesheet for {selectedTimeSheet.Date.ToShortDateString()} needs correction";
                    _notificationService.NotifyUser("Timesheet Needs Correction", message, submittedUser.Code);
                }

                // Send Email, Email Template name is hard corded - Need to change later
                // Replace the hard coded emails with settings or a team.
#if !DEBUG
                    _emailComposerService.TimeSheetNeedsCorrection(selectedTimeSheet.Id);
#endif

                return Json(true);
            }

            return Json(false);
        }

        [HttpGet]
        public ActionResult GetMyTeamSheets(MyTeamCalendarSearchViewModel vm)
        {
            Func<IQueryable<TimeSheet>, IQueryable<TimeSheet>> timeSheetFilter = q =>
            {
                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.Date >= vm.start.Value);
                }

                if (vm.end.HasValue)
                {
                    q = q.Where(t => t.Date <= vm.end.Value);
                }

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(t => t.CreatedByUserId == vm.SubmittedUserById.Value);
                }
                else
                {
                    // Get only mine and my reportees

                    var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                    var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.UserId).ToList();
                    var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id), "Person").ToList();
                    var userListIds = userList.Select(u => u.Id).ToList();

                    q = q.Where(t => userListIds.Contains(t.CreatedByUserId));
                }

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.State == vm.State.Value);
                }

                return q;
            };

            var payload = _timeSheetRepository.Search(timeSheetFilter).ToList().Select(h => new TimeSheetCalenderEvent
            {
                id = h.Id,
                allDay = true,
                title = h.Title,
                description = $"{h.Title} - {h.TotalHours} hours - {h.State}",
                start = h.Date.ToString("s"),
                end = h.Date.ToString("s"),
                state = h.State
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult MyTimeSheet()
        {
            return View();
        }

        public ActionResult Index(TimeSheetSearchViewModel vm)
        {
            vm.SubmittedUserById = WebUser.Id;
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id);
            vm.HasTeam = _employeeRepository.Any(u => u.ReportingPersonId == employee.Id);

            Func<IQueryable<TimeSheet>, IQueryable<TimeSheet>> timeSheetFilter = q =>
            {
                q = q.Where(r => r.CreatedByUserId == WebUser.Id);

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.Date >= vm.StartDate.Value);
                }
                else
                {
                    if (!vm.IsPostBack)
                    {
                        if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                        {
                            vm.StartDate = DateTime.Today;
                            q = q.Where(r => r.Date >= vm.StartDate.Value);
                        }
                        else
                        {
                            var monday = DateTime.Today.Previous(DayOfWeek.Monday);
                            vm.StartDate = monday;
                            q = q.Where(r => r.Date >= vm.StartDate.Value);
                        }
                    }
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.Date <= vm.EndDate.Value);
                }
                else
                {
                    if (!vm.IsPostBack)
                    {
                        if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                        {
                            vm.EndDate = DateTime.Today;
                            q = q.Where(r => r.Date <= vm.EndDate.Value);
                        }
                        else
                        {
                            var friday = DateTime.Today.Next(DayOfWeek.Friday);
                            vm.EndDate = friday;
                            q = q.Where(r => r.Date <= vm.EndDate.Value);
                            vm.IsPostBack = true;
                        }

                    }
                }

                q = q.OrderByDescending(d => d.Date);

                return q;
            };

            vm.TimeSheets = _timeSheetRepository.Search(timeSheetFilter).ToList().Select(u => new TimeSheetViewModel(u, WebUser)).ToList();

            var thisMonday = DateTime.Today.Previous(DayOfWeek.Monday);
            var thisFriday = DateTime.Today.Next(DayOfWeek.Friday);

            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            {
                thisMonday = DateTime.Today;
            }

            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            {
                thisFriday = DateTime.Today;
            }

            var totalHoursInTimeSheet = _timeSheetRepository.GetAllBy(r => r.CreatedByUserId == WebUser.Id && r.Date >= thisMonday && r.Date <= thisFriday).ToList().Sum(r => r.TotalHours);

            ViewBag.TimeSheetHours = totalHoursInTimeSheet * 100 / 40;

            return View(vm);
        }

        public ActionResult Missed()
        {
            var missedSheets = _missedTimeSheetRepository.GetAllBy(m => m.UserId == WebUser.Id && m.FilledOn == null, o => o.OrderByDescending(t => t.Date)).ToList();
            return View(missedSheets);
        }

        public ActionResult TeamMissed()
        {
            // Get only mine and my reportees
            var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
            var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.Id).ToList();
            var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id), "Person").Select(u => u.Id).ToList();
            var missedSheets = _missedTimeSheetRepository.GetAllBy(r => userList.Contains(r.UserId) && r.FilledOn == null, o => o.OrderBy(t => t.UserId), "User.Person").ToList();

            return View(missedSheets);
        }

        public ActionResult MyTeam(TimeSheetSearchViewModel vm)
        {
            // Get only mine and my reportees

            var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
            var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.UserId).ToList();
            var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id) || u.Id == WebUser.Id, "Person").ToList();
            ViewBag.SubmittedUserById = new SelectList(userList, "Id", "Person.Name", vm.SubmittedUserById);

            var myProjects = _projectMemberRepository.GetAllBy(m => m.EmployeeId == loginEmployee.Id && m.MemberStatus == MemberStatus.Active, "Project").Select(m => m.Project).ToList();
            var getOnlyOpenProjects = myProjects.Where(p => !(p.Status == ProjectStatus.Cancelled || p.Status == ProjectStatus.Closed)).OrderBy(o=>o.Title).ToList();
            ViewBag.ProjectId = new SelectList(getOnlyOpenProjects, "Id", "Title", vm.ProjectId);


            if (WebUser.IsAdmin || PermissionChecker.CheckPermission(WebUser.Permissions, 210))
            {
                 ViewBag.SubmittedUserById = new SelectList(_userRepository.GetAllBy(u => u.Id != 1, "Person").ToList().OrderBy(p => p.Person.Name), "Id", "Person.Name", vm.SubmittedUserById);

            }



            Func<IQueryable<TimeSheet>, IQueryable<TimeSheet>> timeSheetFilter = q =>
            {
                q = q.Include("CreatedByUser.Person");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.CreatedByUserId == vm.SubmittedUserById.Value);
                }

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.Date >= vm.StartDate.Value);
                }
                else
                {
                    if (!vm.IsPostBack)
                    {
                        if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                        {
                            vm.StartDate = DateTime.Today;
                            q = q.Where(r => r.Date >= vm.StartDate.Value);
                        }
                        else
                        {
                            var monday = DateTime.Today.Previous(DayOfWeek.Monday);
                            vm.StartDate = monday;
                            q = q.Where(r => r.Date >= vm.StartDate.Value);
                        }
                    }


                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.Date <= vm.EndDate.Value);
                }
                else
                {
                    if (!vm.IsPostBack)
                    {
                        if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                        {
                            vm.EndDate = DateTime.Today;
                            q = q.Where(r => r.Date <= vm.EndDate.Value);
                        }
                        else
                        {
                            var friday = DateTime.Today.Next(DayOfWeek.Friday);
                            vm.EndDate = friday;
                            q = q.Where(r => r.Date <= vm.EndDate.Value);
                            vm.IsPostBack = true;
                        }
                    }
                }
                if (WebUser.IsAdmin || PermissionChecker.CheckPermission(WebUser.Permissions, 210))
                {
                    var user = _userRepository.GetAllBy(u => u.Id != 1, "Person").ToList();
                    var usersListIds = user.Select(u => u.Id).ToList();
                    q = q.Where(r => usersListIds.Contains(r.CreatedByUserId));

                }else
                {
                    var userListIds = userList.Select(u => u.Id).ToList();
                    q = q.Where(r => userListIds.Contains(r.CreatedByUserId));

                   
                }

                q = q.OrderByDescending(d => d.CreatedOn);
                return q;
            };

            vm.TimeSheets = _timeSheetRepository.Search(timeSheetFilter).ToList().Select(u => new TimeSheetViewModel(u, WebUser)).ToList();

            return vm.IsCalendarMode ? View("MyTeamCalendar", vm) : View(vm);
        }

        public ActionResult Download(TimeSheetSearchViewModel vm)
        {
            // Get only mine and my reportees
            var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
            var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.UserId).ToList();
            var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id) || u.Id == WebUser.Id, "Person").ToList();
         

            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetLineItemFilter = q =>
        {
            q = q.Include("TimeSheet.CreatedByUser.Person").Include("TimeSheet.CreatedByUser.Department").Include("Project").Include("Task");

            if (vm.SubmittedUserById.HasValue)
            {
                q = q.Where(r => r.TimeSheet.CreatedByUserId == vm.SubmittedUserById.Value);
            }

            if (vm.State.HasValue)
            {
                q = q.Where(r => r.TimeSheet.State == vm.State.Value);
            }

            if (vm.StartDate.HasValue)
            {
                q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
            }

            if (vm.EndDate.HasValue)
            {
                q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
            }
            if (PermissionChecker.CheckPermission(WebUser.Permissions, 550))
            {
                var user = _userRepository.GetAllBy(u => u.Id != 1, "Person").ToList();
                var usersListIds = user.Select(u => u.Id).ToList();
                q = q.Where(r => usersListIds.Contains(r.TimeSheet.CreatedByUserId));
            }
            else
            {
                var userListIds = userList.Select(u => u.Id).ToList();
                q = q.Where(r => userListIds.Contains(r.TimeSheet.CreatedByUserId));
            }

              

            q = q.OrderByDescending(d => d.CreatedOn);

            return q;
        };

            var timeSheets = _timeSheetLineItemRepository.Search(timeSheetLineItemFilter);
            return ExportAsCSV(timeSheets);
        }

        public ActionResult DownloadMine(TimeSheetSearchViewModel vm)
        {
            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetLineItemFilter = q =>
            {
                q = q.Include("TimeSheet.CreatedByUser.Person").Include("Project").Include("Task");

                q = q.Where(r => r.TimeSheet.CreatedByUserId == WebUser.Id);

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }

                q = q.OrderByDescending(d => d.CreatedOn);

                return q;
            };

            var timeSheets = _timeSheetLineItemRepository.Search(timeSheetLineItemFilter);
            return ExportAsCSV(timeSheets);
        }

        private ActionResult ExportAsCSV(IEnumerable<TimeSheetLineItem> lineItems)
        {
            var sw = new StringBuilder();
            //write the header
            sw.AppendLine("Id,TimeSheetId,Project,Date,TaskId,Task,Effort,Billable,Comments,Created By,Created On,Project Type,Department");

            foreach (var record in lineItems)
            {
                sw.AppendLine($"{record.Id},{record.TimeSheetId},{record.Project.Title.RemoveComma()},{record.TimeSheet.Date.ToShortDateString()},{record.TaskId},{record.TaskSummary.RemoveComma()},{record.Effort},{record.WorkType}, ,{record.TimeSheet.CreatedByUser.Person.Name},{record.CreatedOn},{record.Project.ProjectType},{record.TimeSheet.CreatedByUser.Department.Title}");
            }

            return File(new UTF8Encoding().GetBytes(sw.ToString()), "text/csv", "TimeSheetExport.csv");
        }

        public ActionResult Details(int id)
        {
            var timeSheet = _timeSheetRepository.Get(id, "ApprovedByEmployee.User.Person");

            if (timeSheet == null)
            {
                return HttpNotFound();
            }

            var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
            var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.UserId).ToList();
            var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id), "Person").Select(u => u.Id).ToList();

            if (timeSheet.CreatedByUserId == WebUser.Id || userList.Contains(timeSheet.CreatedByUserId) || WebUser.IsAdmin || PermissionChecker.CheckPermission(WebUser.Permissions, 210))
            {
                var timeSheetRows = _timeSheetLineItemRepository.GetAllBy(l => l.TimeSheetId == timeSheet.Id, "Project");
                var activities = _timeSheetActivityRepository.GetAllBy(l => l.TimeSheetId == timeSheet.Id, o => o.OrderByDescending(t => t.CreatedOn));

                var viewModel = new TimeSheetViewModel(timeSheet, WebUser)
                {
                    LineItems = timeSheetRows.ToList(),
                    TimeSheetActivities = activities.ToList()
                };

                // Is the Person a Manager, then only show the Approval Screen.

                var timeSheetUser = _userRepository.Get(timeSheet.CreatedByUserId, "ReportingPerson");
                var timesheetEmployee = _employeeRepository.GetBy(u => u.UserId == timeSheetUser.Id);
                var employee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                viewModel.IsApprover = timeSheetUser != null && timesheetEmployee.ReportingPersonId == employee.Id;

                return View(viewModel);
            }
            else
            {
                return new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Error" },
                    { "action", "NotAuthorized" },
                    {"area", ""}
                });
            }
        }

        public ActionResult Delete(int id)
        {
            var timeSheet = _timeSheetRepository.Get(id);

            if (timeSheet == null)
            {
                return HttpNotFound();
            }

            return View(timeSheet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var timeSheet = _timeSheetRepository.Get(id);
            if (timeSheet != null)
            {
                var lineItems = _timeSheetLineItemRepository.GetAllBy(l => l.TimeSheetId == timeSheet.Id);
                foreach (var lineItem in lineItems)
                {
                    _timeSheetLineItemRepository.Delete(lineItem);
                }
                var activities = _timeSheetActivityRepository.GetAllBy(l => l.TimeSheetId == timeSheet.Id);
                foreach (var activity in activities)
                {
                    _timeSheetActivityRepository.Delete(activity);
                }
                _unitOfWork.Commit();

                //Remove main
                _timeSheetRepository.Delete(timeSheet);
                _unitOfWork.Commit();
            }
            else
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }
    }
}