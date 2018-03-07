using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FluentDateTime;
using Grid.Areas.PMS.Models;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Hubs;
using SelectItem = Grid.Models.SelectItem;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.PMS.ViewModels;
using Grid.Infrastructure.Extensions;
using Grid.Infrastructure.Filters;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Areas.TMS.Controllers
{
    //[GridPermission(PermissionCode = 550)]
    public class TimeSheetsController : TimeSheetBaseController
    {
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;
        private readonly ITimeSheetActivityRepository _timeSheetActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IEmployeeRepository _employeeRepository;

        private readonly INotificationService _notificationService;
        private readonly EmailComposerService _emailComposerService;

        public TimeSheetsController(INotificationService notificationService,
                                    ITimeSheetRepository timeSheetRepository,
                                    ITimeSheetLineItemRepository timeSheetLineItemRepository,
                                    ITimeSheetActivityRepository timeSheetActivityRepository,
                                    IProjectMemberRepository projectMemberRepository,
                                    IUserRepository userRepository,
                                    EmailComposerService emailComposerService,
                                     IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;

            _timeSheetRepository = timeSheetRepository;
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
            _timeSheetActivityRepository = timeSheetActivityRepository;
            _projectMemberRepository = projectMemberRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailComposerService = emailComposerService;
        }

        #region AjaxCalls

        public FileContentResult EffortByProjectCSV(TimeSheetSearchViewModel vm)
        {
            var teamMembers = new List<int>();
            var csv = new StringBuilder();

            if (vm.TeamMode)
            {
                if (WebUser.IsAdmin)
                {
                    teamMembers.AddRange(_userRepository.GetAll().Select(u => u.Id).ToList());
                }
                else
                {

                    var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                    var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.UserId).ToList();
                    teamMembers.AddRange(myReportees);
                }
            }

            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetFilter = q =>
            {
                q = q.Include("Project");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.CreatedByUserId == vm.SubmittedUserById.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }

                if (vm.TeamMode && teamMembers.Any())
                {
                    q = q.Where(r => teamMembers.Contains(r.TimeSheet.CreatedByUserId));
                }

                return q;
            };

            var timeSheetLineItems = _timeSheetLineItemRepository.Search(timeSheetFilter);
            var employeesByLocation = timeSheetLineItems.GroupBy(l => l.Project.Title)
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

        public FileContentResult EffortByBillableCSV(TimeSheetSearchViewModel vm)
        {
            var teamMembers = new List<int>();
            var csv = new StringBuilder();

            if (vm.TeamMode)
            {
                if (WebUser.IsAdmin)
                {
                    teamMembers.AddRange(_userRepository.GetAll().Select(u => u.Id).ToList());
                }
                else
                {
                    var loginEmployee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                    var myReportees = _employeeRepository.GetAllBy(u => u.ReportingPersonId == loginEmployee.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.UserId).ToList();
                    teamMembers.AddRange(myReportees);
                }
            }

            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetFilter = q =>
            {
                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.CreatedByUserId == vm.SubmittedUserById.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }

                if (vm.TeamMode && teamMembers.Any())
                {
                    q = q.Where(r => teamMembers.Contains(r.TimeSheet.CreatedByUserId));
                }

                return q;
            };

            var timeSheetLineItems = _timeSheetLineItemRepository.Search(timeSheetFilter);
            var employeesByLocation = timeSheetLineItems.GroupBy(l => l.WorkType)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               Value = x.Sum(i => i.Effort)
                                           })
                                            .ToList();

            // Bad, we are getting numbers. So I put the name instead
            csv.AppendLine("Non-Billable, Billable");
            var values = string.Join(",", employeesByLocation.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EffortByBillableCSV.csv");
        }

        [HttpGet]
        public ActionResult GetAllProjectsForTimeSheet()
        {
            // Get only Projects to which i am added
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var myProjects = _projectMemberRepository.GetAllBy(m => m.EmployeeId == employee.Id, "Project").Select(m => m.Project).ToList();
            var selectItems = myProjects.Select(p => new SelectItem
            {
                Id = p.Id,
                Title = p.Title
            });

            return Json(selectItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostSheet(TimeSheetModel timeSheet)
        {
            var date = DateTime.ParseExact(timeSheet.Date, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            var newTimeSheet = new TimeSheet
            {
                State = TimeSheetState.PendingApproval,
                Title = $"{WebUser.Name}\'s TimeSheet for  {date.Date.ToShortDateString()}",
                Date = date,
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
                    TaskSummary = lineItem.TaskSummary,
                    Effort = lineItem.Effort,
                    Comments = lineItem.Comments,
                    WorkType = lineItem.WorkType
                };

                _timeSheetLineItemRepository.Create(newTimeSheetLineItem);
            }

            _unitOfWork.Commit();

            return Json(true);
        }

        [HttpGet]
        public ActionResult GetAllSheets(EventDataFilter vm)
        {
            Func<IQueryable<TimeSheet>, IQueryable<TimeSheet>> timeSheetFilter = q =>
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

            var timeSheets = _timeSheetRepository.Search(timeSheetFilter);

            var payload = timeSheets.ToList().Select(h => new EventData
            {
                id = h.Id,
                allDay = true,
                title = $"{h.Date.ToShortDateString()} - {h.TotalHours} hours - {h.State}",
                start = h.Date.ToString("s"),
                end = h.Date.ToString("s")
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
                    Title = timeSheet.Title,
                    Date = timeSheet.Date.ToString("MM/dd/yyyy"),
                    Comments = timeSheet.Comments
                };

                var lineItemModels = lineItems.Select(l => new TimeSheetLineItemModel
                {
                    Comments = l.Comments,
                    Effort = l.Effort,
                    ProjectId = l.ProjectId,
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
                    Comment = $"{WebUser.Name} approved the TimeSheet at {DateTime.UtcNow:G} with comments {vm.Comments}",
                    CreatedByUserId = WebUser.Id
                };

                _timeSheetActivityRepository.Create(activity);
                _unitOfWork.Commit();

                // Notify the user online
                var submittedByUser = _userRepository.Get(selectedTimeSheet.CreatedByUserId);
                if (submittedByUser?.Person != null)
                {
                    var message = $"Your timesheet for {selectedTimeSheet.Date.ToShortDateString()} has been approved";
                    _notificationService.NotifyUser("Timesheet Approved", message, submittedByUser.Code);
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
                    Comment = $"{WebUser.Name} rejected the TimeSheet at {DateTime.UtcNow:G} with comments {vm.Comments}",
                    CreatedByUserId = WebUser.Id
                };

                _timeSheetActivityRepository.Create(activity);
                _unitOfWork.Commit();

                // Notify the user online
                var submittedByUser = _userRepository.Get(selectedTimeSheet.CreatedByUserId);
                if (submittedByUser?.Person != null)
                {
                    var message = $"Your timesheet for {selectedTimeSheet.Date.ToShortDateString()} needs correction";
                    _notificationService.NotifyUser("Timesheet Needs Correction", message, submittedByUser.Code);
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
        #endregion
        public ActionResult TimesheetManage()
        {            
            return View();
        }

        public ActionResult Index(TimeSheetSearchViewModel vm)
        {
            ViewBag.SubmittedUserById = new SelectList(_userRepository.GetAllBy(u => u.Id != 1, "Person").ToList().OrderBy(p => p.Person.Name), "Id", "Person.Name", vm.SubmittedUserById);

            
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
                        var monday = DateTime.Today.Previous(DayOfWeek.Monday);
                        vm.StartDate = monday;
                        q = q.Where(r => r.Date >= vm.StartDate.Value);
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
                        var friday = DateTime.Today.Next(DayOfWeek.Friday);
                        vm.EndDate = friday;
                        q = q.Where(r => r.Date <= vm.EndDate.Value);
                        vm.IsPostBack = true;
                    }
                }

                q = q.OrderByDescending(d => d.Date).ThenBy(d => d.CreatedOn);

                return q;
            };

            var timeSheets = _timeSheetRepository.Search(timeSheetFilter);

            vm.TimeSheets = timeSheets.Select(u => new TimeSheetViewModel(u, WebUser)).ToList();
            return View(vm);
        }

        public ActionResult Download(TimeSheetSearchViewModel vm)
        {
            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetItemFilter = q =>
            {
                q = q.Include("TimeSheet.CreatedByUser.Person").Include("Project").Include("Task");

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

                q = q.OrderByDescending(d => d.CreatedOn);
                return q;
            };

            var timeSheetItems = _timeSheetLineItemRepository.Search(timeSheetItemFilter);

            return ExportAsCSV(timeSheetItems);
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
            var timeSheet = _timeSheetRepository.Get(id, "ApprovedByEmployee.User.Person,CreatedByUser.Person");
            var timeSheetRows = _timeSheetLineItemRepository.GetAllBy(l => l.TimeSheetId == timeSheet.Id, "Project");

            var viewModel = new TimeSheetViewModel(timeSheet, WebUser)
            {
                LineItems = timeSheetRows.ToList()
            };

            // Is the Person a Manager, then only show the Approval Screen.

            var timeSheetUser = _userRepository.Get(timeSheet.CreatedByUserId, "ReportingPerson");
            var timesheetEmployee = _employeeRepository.GetBy(u => u.UserId == timeSheetUser.Id);
            var employee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
            viewModel.IsApprover = timeSheetUser != null && timesheetEmployee.ReportingPersonId == employee.Id;
            return View(viewModel);
        }

        public ActionResult Delete(int id)
        {
            var timeSheet = _timeSheetRepository.Get(id);
            return CheckForNullAndExecute(timeSheet, () => View(timeSheet));
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



