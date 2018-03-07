using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.LMS;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Newtonsoft.Json;
using Grid.Features.LMS.Entities;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.ViewModels;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.LMS.Services.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using System.Collections.Generic;
using Grid.Data;

namespace Grid.Api.Controllers
{
    public class LeavesController : GridApiBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveEntitlementRepository _leaveEntitlementRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveService _leaveService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailComposerService _emailComposerService;
        private readonly ILeaveTimePeriodRepository _leaveTimePeriodRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;
        private readonly ITimeSheetActivityRepository _timeSheetActivityRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly GridDataContext _dataContext;

        public LeavesController(IUserRepository userRepository,
                                ILeaveRepository leaveRepository,
                                ILeaveEntitlementRepository leaveEntitlementRepository,
                                IEmployeeRepository employeeRepository,
                                ILeaveEntitlementUpdateRepository leaveEntitlementUpdateRepository,
                                ILeaveService leaveService,
                                EmailComposerService emailComposerService,
                                ILeaveTimePeriodRepository leaveTimePeriodRepository,
                                ITimeSheetRepository timeSheetRepository,
                                ITimeSheetLineItemRepository timeSheetLineItemRepository,
                                ITimeSheetActivityRepository timeSheetActivityRepository,
                                IProjectRepository projectRepository,
                                GridDataContext dataContext,
                                IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _leaveRepository = leaveRepository;
            _leaveEntitlementRepository = leaveEntitlementRepository;
            _leaveService = leaveService;
            _employeeRepository = employeeRepository;
            _emailComposerService = emailComposerService;
            _leaveTimePeriodRepository = leaveTimePeriodRepository;
            _timeSheetRepository = timeSheetRepository;
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
            _timeSheetActivityRepository = timeSheetActivityRepository;
            _projectRepository = projectRepository;
            _dataContext = dataContext;
            _unitOfWork = unitOfWork;
        }
        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _leaveRepository.GetAll().Select(h => new LeaveModel(h)).ToList();
            }, "Leaves Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _leaveRepository.Get(id, "CreatedByUser.Person,RequestedForUser.User.Person,Approver.User.Person,LeaveType"), "Leave fetched sucessfully");
            var json = JsonConvert.SerializeObject(apiResult, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(json, "application/json");
        }


        [HttpGet]
        public ActionResult CheckShowApprove(int id)
        {
            var apiResult = TryExecute(() =>
            {

                var leave = _leaveRepository.Get(id, "CreatedByUser.Person,RequestedForUser.User.Person,Approver.User.Person,LeaveType");
                var currentEmployee = _employeeRepository.GetBy(l => l.UserId == WebUser.Id);

                if (leave.ApproverId == currentEmployee.Id)
                {
                    return new { Leave = leave, IsApprove = true };
                }
                return new { Leave = leave, IsApprove = false };


            }, "Leaves Fetched sucessfully");

            var json = JsonConvert.SerializeObject(apiResult, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(json, "application/json");
        }

        [HttpPost]
        public JsonResult CheckLeaveBalance(Leave vm)
        {

            ApiResult<LeaveBalanceModel> result;

            Employee currentUser = new Employee();

            if (vm.RequestedForUserId != 0)
            {
                 currentUser = _employeeRepository.GetBy(l => l.Id == vm.RequestedForUserId, "User.Person,ReportingPerson");

            }
            else
            {
                currentUser = _employeeRepository.GetBy(l => l.UserId == WebUser.Id, "User.Person,ReportingPerson");

            }

            if (vm.Duration != LeaveDuration.MultipleDays)
            {
                vm.End = vm.Start;
                var timeTrimmed = vm.Start.Date;

                // Satuday & Sunday is holidays for us.
                if (timeTrimmed.DayOfWeek == DayOfWeek.Saturday || timeTrimmed.DayOfWeek == DayOfWeek.Sunday)
                {
                    result = new ApiResult<LeaveBalanceModel>
                    {
                        Status = false,
                        Message = "Applied leave date comes under the holiday list.",
                    };

                    return Json(result);
                }
            }
            if (vm.End >= vm.Start)
            {
                if (currentUser.ReportingPersonId.HasValue)
                {
                    float leaveCount;

                    // Short Circuit - Deduct only half for half days
                    if (vm.Duration == LeaveDuration.FirstHalf || vm.Duration == LeaveDuration.SecondHalf)
                    {
                        vm.End = vm.Start;
                        leaveCount = 0.5f;
                    }
                    else if (vm.Duration == LeaveDuration.OneFullDay)
                    {
                        vm.End = vm.Start;
                        leaveCount = 1;
                    }

                    else
                    {
                        leaveCount = _leaveService.GetLeaveCount(vm.Start, vm.End);
                    }

                    float allocation = 0;
                    var leavePeriod = _leaveTimePeriodRepository.GetBy(i => i.Start <= DateTime.UtcNow && i.End >= DateTime.UtcNow);
                    if (vm.RequestedForUserId != 0)
                    {
                        allocation = _leaveEntitlementRepository.GetAllBy(l => l.LeaveTypeId == vm.LeaveTypeId && l.EmployeeId == vm.RequestedForUserId && l.LeaveTimePeriodId == leavePeriod.Id).Sum(i => i.Allocation);
                    }
                    else
                    {
                        var employee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                        allocation = _leaveEntitlementRepository.GetAllBy(l => l.LeaveTypeId == vm.LeaveTypeId && l.EmployeeId == employee.Id && l.LeaveTimePeriodId == leavePeriod.Id).Sum(i => i.Allocation);
                    }
                    if (allocation != 0)
                    {
                        var hasBalance = allocation >= leaveCount;
                        if (hasBalance)
                        {
                            result = new ApiResult<LeaveBalanceModel>
                            {
                                Status = true,
                                Message = "Success",
                                Result = new LeaveBalanceModel
                                {
                                    Allocation = allocation,
                                    LeaveCount = leaveCount,
                                    End = vm.End
                                }
                            };

                            return Json(result);
                        }

                        result = new ApiResult<LeaveBalanceModel>
                        {
                            Status = false,
                            Message = $"You don't have sufficient Leave Balance. You have only {allocation} while you are applying for {leaveCount}",
                            Result = new LeaveBalanceModel
                            {
                                Allocation = allocation,
                                LeaveCount = leaveCount,
                                End = vm.End
                            }
                        };

                        return Json(result);
                    }

                    result = new ApiResult<LeaveBalanceModel>
                    {
                        Status = false,
                        Message = "You don't have any Entitlements for this Leave Type",
                        Result = new LeaveBalanceModel
                        {
                            LeaveCount = leaveCount
                        }
                    };

                    return Json(result);
                }
                else
                {
                    result = new ApiResult<LeaveBalanceModel>
                    {
                        Status = false,
                        Message = "You don't have any Reporting Person to apply leave...!",
                    };

                    return Json(result);
                }
            }
            else
            {
                result = new ApiResult<LeaveBalanceModel>
                {
                    Status = false,
                    Message = "End date should be greater than start date...!",

                };

                return Json(result);
            }
        }

        [HttpPost]
        public ActionResult Update(LeaveUpdateModel vm)
        {

            if (ModelState.IsValid)
            {
                Employee currentUser = new Employee();

         
                if (vm.RequestedForUserId.Value != 0)
                {
                    currentUser = _employeeRepository.GetBy(l => l.Id == vm.RequestedForUserId);
                }
                else
                {
                    currentUser = _employeeRepository.GetBy(l => l.UserId == WebUser.Id);

                }
                if (currentUser.ReportingPersonId.HasValue)

                {
                    if (currentUser != null)
                    {
                        // Preprocess based on Duration, we dont get the end if it's a single day or half day
                        if (vm.Duration != LeaveDuration.MultipleDays)
                        {
                            vm.End = vm.Start;
                        }
                        var newLeave = new Leave
                        {
                            LeaveTypeId = vm.LeaveTypeId,
                            Duration = vm.Duration,
                            Start = vm.Start,
                            End = vm.End,
                            Reason = vm.Reason,
                            Status = LeaveStatus.Pending,
                            ApproverId = currentUser.ReportingPersonId ?? 2,
                            Count = vm.Count,
                            CreatedByUserId = WebUser.Id
                        };
                        if (vm.RequestedForUserId.HasValue && vm.RequestedForUserId != 0)
                        {
                            newLeave.RequestedForUserId = vm.RequestedForUserId;
                        }
                        else
                        {
                            var employee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
                            newLeave.RequestedForUserId = employee.Id;
                        }
                        _leaveRepository.Create(newLeave);
                        _unitOfWork.Commit();

                        var employeeId = newLeave.RequestedForUserId;
                        var loginEmployee = _employeeRepository.GetBy(r => r.Id == employeeId);
                        var userEmployee = _userRepository.GetBy(r => r.Id == loginEmployee.UserId, "Person");

                        if (vm.Duration == LeaveDuration.FirstHalf || vm.Duration == LeaveDuration.SecondHalf)
                        {
                            var date = vm.Start;
                            var exists = _timeSheetRepository.Any(t => t.CreatedByUserId == userEmployee.Id && t.Date == date.Date);
                            if (!exists)
                            {
                                var newTimeSheet = new TimeSheet
                                {
                                    State = TimeSheetState.PendingApproval,
                                    Title = $"{userEmployee.Person.Name}'s {"TimeSheet for "} {vm.Start.ToShortDateString()}",
                                    Date = date.Date,
                                    TotalHours = 4,
                                    Comments = null,
                                    CreatedByUserId = userEmployee.Id
                                };

                                _timeSheetRepository.Create(newTimeSheet);

                                var project = _projectRepository.GetBy(i => i.Title == "Leave");
                                var newTimeSheetLineItem = new TimeSheetLineItem
                                {
                                    TimeSheetId = newTimeSheet.Id,
                                    ProjectId = project.Id,
                                    TaskId = null,
                                    TaskSummary = "Leave",
                                    Effort = 4,
                                    Comments = null,
                                    WorkType = 2
                                };
                                _timeSheetLineItemRepository.Create(newTimeSheetLineItem);
                                _unitOfWork.Commit();

                                // Log Activity 
                                var activity = new TimeSheetActivity
                                {
                                    TimeSheetId = newTimeSheet.Id,
                                    Title = "Created",
                                    Comment = $"{userEmployee.Person.Name} created the TimeSheet at {DateTime.UtcNow.ToString("G")} with state {newTimeSheet.State} & hours {newTimeSheet.TotalHours}",
                                    CreatedByUserId = userEmployee.Id
                                };
                                _timeSheetActivityRepository.Create(activity);
                                _unitOfWork.Commit();
                            }
                        }
                        if (vm.Duration == LeaveDuration.OneFullDay)
                        {
                            var date = vm.Start;
                            var exists = _timeSheetRepository.Any(t => t.CreatedByUserId == userEmployee.Id && t.Date == date.Date);
                            if (!exists)
                            {
                                var newTimeSheet = new TimeSheet
                                {
                                    State = TimeSheetState.PendingApproval,
                                    Title = $"{userEmployee.Person.Name}'s {"TimeSheet for "} {vm.Start.ToShortDateString()}",
                                    Date = date.Date,
                                    TotalHours = 8,
                                    Comments = null,
                                    CreatedByUserId = userEmployee.Id
                                };
                                _timeSheetRepository.Create(newTimeSheet);

                                var project = _projectRepository.GetBy(i => i.Title == "Leave");
                                var newTimeSheetLineItem = new TimeSheetLineItem
                                {
                                    TimeSheetId = newTimeSheet.Id,
                                    ProjectId = project.Id,
                                    TaskId = null,
                                    TaskSummary = "Leave",
                                    Effort = 8,
                                    Comments = null,
                                    WorkType = 2
                                };
                                _timeSheetLineItemRepository.Create(newTimeSheetLineItem);
                                _unitOfWork.Commit();

                                var activity = new TimeSheetActivity
                                {
                                    TimeSheetId = newTimeSheet.Id,
                                    Title = "Created",
                                    Comment = $"{userEmployee.Person.Name} created the TimeSheet at {DateTime.UtcNow.ToString("G")} with state {newTimeSheet.State} & hours {newTimeSheet.TotalHours}",
                                    CreatedByUserId = userEmployee.Id
                                };
                                _timeSheetActivityRepository.Create(activity);
                                _unitOfWork.Commit();
                            }
                        }
                        if (vm.Duration == LeaveDuration.MultipleDays)
                        {
                            List<DateTime> allDates = new List<DateTime>();
                            List<DateTime> datesForTimesheet = new List<DateTime>();
                            for (DateTime dates = vm.Start; dates <= vm.End; dates = dates.AddDays(1))
                            {
                                allDates.Add(dates);
                            }
                            foreach (var datetime in allDates)
                            {
                                if (datetime.DayOfWeek != DayOfWeek.Saturday && datetime.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    datesForTimesheet.Add(datetime);
                                }
                            }
                            var date = vm.Start;
                            var exists = _timeSheetRepository.Any(t => t.CreatedByUserId == userEmployee.Id && t.Date == date.Date);
                            if (!exists)
                            {
                                foreach (var timesheetdate in datesForTimesheet)
                                {
                                    var newTimeSheet = new TimeSheet
                                    {
                                        State = TimeSheetState.PendingApproval,
                                        Title = $"{userEmployee.Person.Name}'s {"TimeSheet for "}{timesheetdate.ToShortDateString()}",
                                        Date = timesheetdate,
                                        TotalHours = 8,
                                        Comments = null,
                                        CreatedByUserId = userEmployee.Id
                                    };
                                    _timeSheetRepository.Create(newTimeSheet);

                                    var project = _projectRepository.GetBy(i => i.Title == "Leave");
                                    var newTimeSheetLineItem = new TimeSheetLineItem
                                    {
                                        TimeSheetId = newTimeSheet.Id,
                                        ProjectId = project.Id,
                                        TaskId = null,
                                        TaskSummary = "Leave",
                                        Effort = 8,
                                        Comments = null,
                                        WorkType = 2
                                    };
                                    _timeSheetLineItemRepository.Create(newTimeSheetLineItem);
                                    _unitOfWork.Commit();

                                    // Log Activity 
                                    var activity = new TimeSheetActivity
                                    {
                                        TimeSheetId = newTimeSheet.Id,
                                        Title = "Created",
                                        Comment = $"{userEmployee.Person.Name} created the TimeSheet at {DateTime.UtcNow.ToString("G")} with state {newTimeSheet.State} & hours {newTimeSheet.TotalHours}",
                                        CreatedByUserId = userEmployee.Id
                                    };
                                    _timeSheetActivityRepository.Create(activity);
                                    _unitOfWork.Commit();
                                }
                            }
                        }

#if !DEBUG
                       // _emailComposerService.TestEmailSend(vm);
                        _emailComposerService.LeaveApplicationEmail(newLeave.Id);
                      //  _emailComposerService.TimeSheetSubmitted(newTimeSheet.Id);
#endif

                        return Json(true);
                    }
                }
            }


            return Json(false);
        }
        public JsonResult Approve(ApproveRejectLeaveViewModel vm)
        {
            ApiResult<ApproveRejectLeaveViewModel> result;

            var leave = _leaveRepository.GetBy(r => r.Id == vm.Id, "RequestedForUser.User.Person");
            var employee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
            var status = _leaveService.Approve(vm.Id, employee.Id, vm.ApproverComments, false);
            if (status)
            {
#if !DEBUG
                   _emailComposerService.LeaveApprovalEmail(vm.Id, false);
#endif

                result = new ApiResult<ApproveRejectLeaveViewModel>
                {
                    Status = true,
                    Message = "Leave Approved",
                    Result = vm
                };

            }
            else
            {
                result = new ApiResult<ApproveRejectLeaveViewModel>
                {
                    Status = false,
                    Message = "Not Enough Balance to Approve the Leave",
                    Result = null
                };
            }


            return Json(result);
        }
        public JsonResult Reject(ApproveRejectLeaveViewModel vm)
        {

            ApiResult<ApproveRejectLeaveViewModel> result;

            var leave = _leaveRepository.GetBy(r => r.Id == vm.Id, "RequestedForUser.User.Person");
            var employee = _employeeRepository.GetBy(r => r.UserId == WebUser.Id);
            var status = _leaveService.Reject(vm.Id, employee.Id, vm.ApproverComments);
            if (status)
            {
#if !DEBUG
                           _emailComposerService.LeaveApprovalEmail(vm.Id, false);
#endif

                result = new ApiResult<ApproveRejectLeaveViewModel>
                {
                    Status = true,
                    Message = "Leave Rejected",
                    Result = vm
                };
            }
            else
            {
                result = new ApiResult<ApproveRejectLeaveViewModel>
                {
                    Status = false,
                    Message = "Leave Can't be Rejected",
                    Result = vm
                };
            }

            return Json(result);
        }
  
        [HttpPost]
        public JsonResult Delete(long id)
        {
            var apiResult = TryExecute(() =>
            {

                var leave = _leaveRepository.GetBy(r => r.Id == id, "CreatedByUser.Person,RequestedForUser.User.Person");
               
                if (leave.Duration != LeaveDuration.MultipleDays)
                {
                    var timesheetDate = _timeSheetRepository.GetAllBy(i => i.CreatedByUserId == leave.RequestedForUser.User.Id).ToList();
                    if(timesheetDate.Count != 0)
                    {
                        var timesheets = timesheetDate.Where(i => i.Date.Date == leave.Start.Date).FirstOrDefault();
                        var timesheetactivity = _timeSheetActivityRepository.GetBy(i => i.CreatedByUserId == leave.RequestedForUser.User.Id && i.TimeSheetId == timesheets.Id);
                        var timeSheetLineItem = _timeSheetLineItemRepository.GetBy(i => i.TimeSheetId == timesheets.Id, "Timesheet");

                        _timeSheetActivityRepository.Delete(timesheetactivity);
                        _timeSheetLineItemRepository.Delete(timeSheetLineItem);
                        _timeSheetRepository.Delete(timesheets);
                    }
                   

                   
                    _leaveRepository.Delete(leave);
                    _unitOfWork.Commit();
                }
                else
                {

                    List<DateTime> allDates = new List<DateTime>();
                    List<DateTime> datesForTimesheet = new List<DateTime>();
                    for (DateTime dates = leave.Start; dates <= leave.End; dates = dates.AddDays(1))
                    {
                        allDates.Add(dates);
                    }
                    foreach (var datetime in allDates)
                    {
                        if (datetime.DayOfWeek != DayOfWeek.Saturday && datetime.DayOfWeek != DayOfWeek.Sunday)
                        {
                            datesForTimesheet.Add(datetime);
                        }
                    }
                    foreach (var timesheetdate in datesForTimesheet)
                    {

                        var timesheetDate = _timeSheetRepository.GetAllBy(i => i.CreatedByUserId == leave.RequestedForUser.User.Id).ToList();
                        var timesheets = timesheetDate.Where(i => i.Date.Date == timesheetdate.Date).FirstOrDefault();

                        var timesheetactivity = _timeSheetActivityRepository.GetBy(i => i.CreatedByUserId == leave.RequestedForUser.User.Id && i.TimeSheetId == timesheets.Id);
                        var timeSheetLineItem = _timeSheetLineItemRepository.GetBy(i => i.TimeSheetId == timesheets.Id, "Timesheet");

                        _timeSheetActivityRepository.Delete(timesheetactivity);
                        _timeSheetLineItemRepository.Delete(timeSheetLineItem);
                        _timeSheetRepository.Delete(timesheets);

                        _unitOfWork.Commit();
                    }
                    _leaveRepository.Delete(leave);
                    _unitOfWork.Commit();

                }
                return true;
            }, "Leave deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);

        }
    }
}
