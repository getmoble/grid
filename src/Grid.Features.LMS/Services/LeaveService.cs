using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.LMS.Services.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Email;

namespace Grid.Features.LMS.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveTimePeriodRepository _leaveTimePeriodRepository;
        private readonly ILeaveEntitlementRepository _leaveEntitlementRepository;
        private readonly ILeaveEntitlementUpdateRepository _leaveEntitlementUpdateRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;

        public LeaveService(ILeaveRepository leaveRepository,
                            IHolidayRepository holidayRepository,
                            ILeaveTimePeriodRepository leaveTimePeriodRepository,
                            ILeaveEntitlementRepository leaveEntitlementRepository,
                            ILeaveEntitlementUpdateRepository leaveEntitlementUpdateRepository,
                            ISettingsService settingsService,
                            IEmployeeRepository employeeRepository,
                            IUserRepository  userRepository, 
                            IUnitOfWork unitOfWork)
        {
            _leaveRepository = leaveRepository;
            _holidayRepository = holidayRepository;
            _leaveEntitlementRepository = leaveEntitlementRepository;
            _leaveEntitlementUpdateRepository = leaveEntitlementUpdateRepository;
            _leaveTimePeriodRepository = leaveTimePeriodRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _settingsService = settingsService;
            _unitOfWork = unitOfWork;
        }

        private KeyValuePair<bool, string> ShouldDeduct(DateTime date)
        {
            var timeTrimmed = date.Date;

            // Satuday & Sunday is holidays for us.
            if (timeTrimmed.DayOfWeek == DayOfWeek.Saturday || timeTrimmed.DayOfWeek == DayOfWeek.Sunday)
            {
                return new KeyValuePair<bool, string>(false, "Sat & Sun is not working days");
            }

            // We are not considering RH holidays now, we will consider later.
            var holiday = _holidayRepository.GetBy(d => d.Date == timeTrimmed);
            return holiday != null ? new KeyValuePair<bool, string>(false, holiday.Title) : new KeyValuePair<bool, string>(true, "Usual Working Day");
        }

        public List<Leave> GetPendingLeaves()
        {
            var currentDate = DateTime.UtcNow.Date;
            var pendingLeaves = _leaveRepository.GetAllBy(l => l.Status == LeaveStatus.Pending && l.End < currentDate);
            return pendingLeaves.ToList();
        }

        public float GetLeaveCount(DateTime startDate, DateTime endDate)
        {
            // Calculate the number of days between these days
            float leaveCount = 0;
            //var counter = startDate.ToUniversalTime();
            var counter = startDate.Date;

            while (counter <= endDate.Date)
            {
                var result = ShouldDeduct(counter);
                if (result.Key)
                {
                    leaveCount = leaveCount + 1;
                }

                counter = counter.AddDays(1);
            }

            return leaveCount;
        }

        public bool Approve(int leaveId, int approverId, string approverComments, bool autoApproved)
        {
            var log = new StringBuilder();

            var leave = _leaveRepository.GetBy(l => l.Id == leaveId, "RequestedForUser.User.Person");

            if (leave != null)
            {
                // Calculate the number of days between these days
                float leaveCount = 0;
                var counter = leave.Start;

                while (counter <= leave.End)
                {
                    var result = ShouldDeduct(counter);
                    if (result.Key)
                    {
                        leaveCount = leaveCount + 1;
                        log.AppendLine($"{counter.ToShortDateString()} - {result.Key} - {result.Value}");
                    }
                    else
                    {
                        log.AppendLine($"{counter.ToShortDateString()} - {result.Key} - {result.Value}");
                    }

                    counter = counter.AddDays(1);
                }

                // Short Circuit - Deduct only half for half days
                if (leave.Duration == LeaveDuration.FirstHalf || leave.Duration == LeaveDuration.SecondHalf)
                {
                    leaveCount = 0.5f;
                }

                // Check for Leave Balance

                var leavePeriod = _leaveTimePeriodRepository.GetBy(i => i.Start <= DateTime.UtcNow && i.End >= DateTime.UtcNow);
                var leaveEntitlement = _leaveEntitlementRepository.GetBy(l => l.LeaveTypeId == leave.LeaveTypeId && l.EmployeeId == leave.RequestedForUserId && l.LeaveTimePeriodId == leavePeriod.Id, "LeaveType");

                //var leaveEntitlement = _leaveEntitlementRepository.GetBy(l => l.LeaveTypeId == leave.LeaveTypeId && l.EmployeeId == leave.RequestedForUserId, "LeaveType");
                if (leaveEntitlement != null)
                {
                    log.AppendLine($"Going to deduct {leaveCount} from {leaveEntitlement.Allocation} leaves of Type {leaveEntitlement.LeaveType.Title}");
                    if (leaveCount <= leaveEntitlement.Allocation)
                    {
                        //Get current
                        var currentLeaveCount = leaveEntitlement.Allocation;

                        // Deduct only half for half days
                        leaveEntitlement.Allocation = leaveEntitlement.Allocation - leaveCount;

                        _leaveEntitlementRepository.Update(leaveEntitlement);
                        _unitOfWork.Commit();

                        // New Balance 
                        log.AppendLine($"New Leave Balance is {leaveEntitlement.Allocation}");

                        // Approve the Leave
                        leave.Status = LeaveStatus.Approved;
                        leave.Count = leaveCount;
                        leave.CalculationLog = log.ToString();
                        leave.ApproverId = approverId;
                        leave.ApproverComments = approverComments;
                        leave.ActedOn = DateTime.UtcNow;

                        _leaveRepository.Update(leave);
                        _unitOfWork.Commit();

                        var leaveApproverId = _employeeRepository.GetBy(i => i.Id == leave.ApproverId);
                        var getUser = leaveApproverId.UserId;

                        var comments = "Deducting leave as leave got approved";

                        // If auto approved, mention that.
                        if (autoApproved)
                        {
                            comments = "Deducting leave as leave got auto approved";
                        }

                      
                        // Create a log
                        var newLeaveEntitlementUpdate = new LeaveEntitlementUpdate
                        {
                            EmployeeId = leave.RequestedForUserId,
                            LeaveTimePeriodId = leaveEntitlement.LeaveTimePeriodId,
                            LeaveTypeId = leave.LeaveTypeId,
                            LeaveId = leave.Id,
                            Operation = LeaveOperation.Deduct,
                            LeaveCount = leave.Count,
                            PreviousBalance = currentLeaveCount,
                            NewBalance = leaveEntitlement.Allocation,
                            Comments = comments,
                            CreatedByUserId = getUser
                        };

                        _leaveEntitlementUpdateRepository.Create(newLeaveEntitlementUpdate);
                        _unitOfWork.Commit();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Reject(int leaveId, int approverId, string approverComments)
        {
            var leave = _leaveRepository.Get(leaveId);
            leave.Status = LeaveStatus.Rejected;
            leave.ApproverId = approverId;
            leave.ApproverComments = approverComments;
            leave.ActedOn = DateTime.UtcNow;
            _leaveRepository.Update(leave);
            _unitOfWork.Commit();
            return true;
        }

        public EmailContext ComposeEmailContextForLeaveApplication(int leaveId)
        {
            var emailContext = new EmailContext();
            var selectedLeave = _leaveRepository.Get(leaveId, "LeaveType");
            if (selectedLeave != null)
            {
                var selectedEmployee = _employeeRepository.Get(selectedLeave.RequestedForUserId.Value, "User,User.Person, ReportingPerson.User.Person");

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[ManagerName]", selectedEmployee.ReportingPerson.User.Person.Name),
                    new PlaceHolder("[Name]", selectedEmployee.User.Person.Name),
                    new PlaceHolder("[LeaveType]", selectedLeave.LeaveType.Title),
                    new PlaceHolder("[StartDate]", selectedLeave.Start.ToShortDateString()),
                    new PlaceHolder("[EndDate]", selectedLeave.End.ToShortDateString()),
                    new PlaceHolder("[Reason]", selectedLeave.Reason),
                    new PlaceHolder("[Url]", $"http://gridapp.azurewebsites.net/LMS/Leaves/Details/{selectedLeave.Id}")
                };

                emailContext.Subject = "Leave Application";

                emailContext.ToAddress.Add(new MailAddress(selectedEmployee.ReportingPerson.OfficialEmail, selectedEmployee.ReportingPerson.User.Person.Name));

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {

                    var selectedPOC = _employeeRepository.Get(settings.POCSettings.HRDepartmentLevel1, "User,User.Person");

                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.User.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForLeaveApproval(int leaveId, bool autoapproved)
        {
            var emailContext = new EmailContext();
            var selectedLeave = _leaveRepository.Get(leaveId, "LeaveType,Approver.User.Person");
            if (selectedLeave != null)
            {
                var selectedEmployee = _employeeRepository.Get(selectedLeave.RequestedForUserId.Value, "User,User.Person, ReportingPerson.User.Person");

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[LeaveType]", selectedLeave.LeaveType.Title),
                    new PlaceHolder("[StartDate]", selectedLeave.Start.ToShortDateString()),
                    new PlaceHolder("[EndDate]", selectedLeave.End.ToShortDateString()),
                    new PlaceHolder("[Reason]", selectedLeave.Reason),
                    new PlaceHolder("[ApproverName]", selectedLeave.Approver.User.Person.Name),
                    new PlaceHolder("[ApproverComments]", selectedLeave.ApproverComments),
                    new PlaceHolder("[Url]", $"http://gridapp.azurewebsites.net/LMS/Leaves/Details/{selectedLeave.Id}")
                };

                emailContext.Subject = "Leave Approved";

                // Update the  subject
                if (autoapproved)
                {
                    emailContext.Subject = "Leave Auto Approved";
                }

                emailContext.ToAddress.Add(new MailAddress(selectedEmployee.OfficialEmail, selectedEmployee.User.Person.Name));

                if (autoapproved)
                {
                    emailContext.CcAddress.Add(new MailAddress(selectedEmployee.ReportingPerson.OfficialEmail, selectedEmployee.ReportingPerson.User.Person.Name));
                }

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    var selectedPOC = _employeeRepository.Get(settings.POCSettings.HRDepartmentLevel1, "User,User.Person");

                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.User.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForLeaveRejected(int leaveId)
        {
            var emailContext = new EmailContext();
            var selectedLeave = _leaveRepository.Get(leaveId, "LeaveType,Approver.User.Person");
            if (selectedLeave != null)
            {
                var selectedEmployee = _employeeRepository.Get(selectedLeave.RequestedForUserId.Value, "User,User.Person, ReportingPerson.User.Person");

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[LeaveType]", selectedLeave.LeaveType.Title),
                    new PlaceHolder("[StartDate]", selectedLeave.Start.ToShortDateString()),
                    new PlaceHolder("[EndDate]", selectedLeave.End.ToShortDateString()),
                    new PlaceHolder("[Reason]", selectedLeave.Reason),
                    new PlaceHolder("[ApproverName]", selectedLeave.Approver.User.Person.Name),
                    new PlaceHolder("[ApproverComments]", selectedLeave.ApproverComments),
                    new PlaceHolder("[Url]", $"http://gridapp.azurewebsites.net/LMS/Leaves/Details/{selectedLeave.Id}")
                };

                emailContext.Subject = "Leave Rejected";

                emailContext.ToAddress.Add(new MailAddress(selectedEmployee.OfficialEmail, selectedEmployee.User.Person.Name));

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    var selectedPOC = _employeeRepository.Get(settings.POCSettings.HRDepartmentLevel1, "User,User.Person");

                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.User.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }
            }

            return emailContext;
        }

        public EmailContext TestEmail(Leave leave)
        {
            var emailContext = new EmailContext();
          
             

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[LeaveType]", leave.Status.ToString()),
                    new PlaceHolder("[StartDate]", leave.Start.ToString()),
                    new PlaceHolder("[EndDate]", leave.End.ToString()),

                };

                emailContext.Subject = "Test Leave";

                emailContext.ToAddress.Add(new MailAddress("syam.kumar@logiticks.com", "Syam Kumar"));

             

            return emailContext;
        }
    }
}
