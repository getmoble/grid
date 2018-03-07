using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using FluentDateTime;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.PMS.Services.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Email;

namespace Grid.Features.PMS.Services
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;
        private readonly IMissedTimeSheetRepository _missedTimeSheetRepository;

        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISettingsService _settingsService;
        private readonly IUnitOfWork _unitOfWork;

        public TimeSheetService(ITimeSheetRepository timeSheetRepository,
                                ITimeSheetLineItemRepository timeSheetLineItemRepository,
                                IMissedTimeSheetRepository missedTimeSheetRepository,
                                IUserRepository userRepository,
                                ISettingsService settingsService,
                                IEmployeeRepository employeeRepository,
                                IUnitOfWork unitOfWork)
        {
            _timeSheetRepository = timeSheetRepository;
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
            _missedTimeSheetRepository = missedTimeSheetRepository;
            _userRepository = userRepository;
            _settingsService = settingsService;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public List<TimeSheet> GetPendingTimeSheets()
        {
            throw new NotImplementedException();
        }

        public bool AutoApprove(int duedays)
        {
            var minusDays = -1 * duedays;
            var expiryDate = DateTime.UtcNow.AddDays(minusDays);
            var pendingSheets = _timeSheetRepository.GetAllBy(t => t.State == TimeSheetState.PendingApproval && t.Date <= expiryDate);
            foreach (var pendingSheet in pendingSheets)
            {
                // Approve here
            }

            return true;
        }

        public EmailContext ComposeEmailContextForTimesheetReminder(int employeeId, DateTime date)
        {
            var emailContext = new EmailContext();

            var selectedEmployee = _userRepository.Get(employeeId, "Person,ReportingPerson.Person,Manager.Person");
          
            // Kind of Bad logic, But will work for now. So let's keep it.
            var settings = _settingsService.GetSiteSettings();
            if (settings.TimeSheetSettings?.MaxTimeSheetMisses > 0)
            {
                    var currentCount = _missedTimeSheetRepository.Count(m => m.UserId == employeeId && m.FilledOn == null);
                    if (currentCount < settings.TimeSheetSettings.MaxTimeSheetMisses)
                    {
                        emailContext.DropEmail = true;
                    }
                }
            
            var timeSheetMisses = _missedTimeSheetRepository.GetAllBy(u => u.UserId == employeeId && u.FilledOn == null).ToList();
            var pendingCorrectionTimeSheets = _timeSheetRepository.GetAllBy(u => u.CreatedByUserId == employeeId && u.State == TimeSheetState.NeedsCorrection).ToList();

            if (selectedEmployee != null)
            {
                var reportCard = new StringBuilder();
                reportCard.AppendLine("<table width='900px' bgcolor='#999999'>");

                var yesterday = DateTime.Today.AddDays(-1);
                reportCard.AppendLine("<tr bgcolor='#ffffff'>");
                reportCard.AppendLine($"<td>{yesterday.Date.DayOfWeek}- {yesterday.Date.ToShortDateString()} - No TimeSheet</td>");
                reportCard.AppendLine("</tr>");

                foreach (var timesheetMiss in timeSheetMisses)
                {
                    reportCard.AppendLine("<tr bgcolor='#ffffff'>");
                    reportCard.AppendLine($"<td>{timesheetMiss.Date.DayOfWeek}- {timesheetMiss.Date.ToShortDateString()} - No TimeSheet</td>");
                    reportCard.AppendLine("</tr>");
                }

                foreach (var pendingTimesheet in pendingCorrectionTimeSheets)
                {
                    reportCard.AppendLine("<tr bgcolor='#ffffff'>");
                    reportCard.AppendLine($"<td>{pendingTimesheet.Date.DayOfWeek}- {pendingTimesheet.Date.ToShortDateString()} - Needs Correction</td>");
                    reportCard.AppendLine("</tr>");
                }

                reportCard.AppendLine("</table>");

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Name]", selectedEmployee.Person.Name),
                    new PlaceHolder("[Report]", reportCard.ToString())
                };

                emailContext.Subject = "Timesheet Reminder";

                emailContext.ToAddress.Add(new MailAddress(selectedEmployee.OfficialEmail, selectedEmployee.Person.Name));
                if (selectedEmployee.ReportingPerson != null)
                {
                    if (selectedEmployee.ReportingPerson.OfficialEmail != null)
                    {
                        emailContext.CcAddress.Add(new MailAddress(selectedEmployee.ReportingPerson.OfficialEmail, selectedEmployee.ReportingPerson.Person.Name));
                    }
                }
                if (selectedEmployee.Manager != null)
                {
                    if (selectedEmployee.Manager.OfficialEmail != null)
                    {
                        emailContext.CcAddress.Add(new MailAddress(selectedEmployee.Manager.OfficialEmail, selectedEmployee.Manager.Person.Name));
                    }
                }
            }

            return emailContext;
        }
            
        public EmailContext ComposeEmailContextForTimesheetMissed(int employeeId, DateTime date)
        {
            var emailContext = new EmailContext();

            var selectedEmployee = _userRepository.Get(employeeId, "Person,ReportingPerson.Person,Manager.Person");

            // Kind of Bad logic, But will work for now. So let's keep it.
            var settings = _settingsService.GetSiteSettings();
            if (settings.TimeSheetSettings?.MaxTimeSheetMisses > 0)
            {
                var currentCount = _missedTimeSheetRepository.Count(m => m.UserId == employeeId && m.FilledOn == null);
                if (currentCount < settings.TimeSheetSettings.MaxTimeSheetMisses)
                {
                    emailContext.DropEmail = true;
                }
            }

            // Check whether it's already there ? 
            var exists = _missedTimeSheetRepository.Any(m => m.UserId == employeeId && m.Date == date && m.FilledOn == null);
            if (!exists)
            {
                // Add it as a missed TimeSheet
                var missedTimeSheet = new MissedTimeSheet
                {
                    Date = date,
                    UserId = employeeId
                };

                _missedTimeSheetRepository.Create(missedTimeSheet);
                _unitOfWork.Commit();
            }

            var timeSheetMisses = _missedTimeSheetRepository.GetAllBy(u => u.UserId == employeeId && u.FilledOn == null).ToList();
            var pendingCorrectionTimeSheets = _timeSheetRepository.GetAllBy(u => u.CreatedByUserId == employeeId && u.State == TimeSheetState.NeedsCorrection).ToList();

            if (selectedEmployee != null)
            {
                var reportCard = new StringBuilder();
                reportCard.AppendLine("<table width='900px' bgcolor='#999999'>");


                foreach (var timesheetMiss in timeSheetMisses)
                {
                    reportCard.AppendLine("<tr bgcolor='#ffffff'>");
                    reportCard.AppendLine($"<td>{timesheetMiss.Date.DayOfWeek}- {timesheetMiss.Date.ToShortDateString()} - No TimeSheet</td>");
                    reportCard.AppendLine("</tr>");
                }

                foreach (var pendingTimesheet in pendingCorrectionTimeSheets)
                {
                    reportCard.AppendLine("<tr bgcolor='#ffffff'>");
                    reportCard.AppendLine($"<td>{pendingTimesheet.Date.DayOfWeek}- {pendingTimesheet.Date.ToShortDateString()} - Needs Correction</td>");
                    reportCard.AppendLine("</tr>");
                }

                reportCard.AppendLine("</table>");

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Name]", selectedEmployee.Person.Name),
                    new PlaceHolder("[Report]", reportCard.ToString())
                };

                emailContext.Subject = "Timesheet Missed !";

                emailContext.ToAddress.Add(new MailAddress(selectedEmployee.OfficialEmail, selectedEmployee.Person.Name));
                if (selectedEmployee.ReportingPerson != null)
                {
                    emailContext.CcAddress.Add(new MailAddress(selectedEmployee.ReportingPerson.OfficialEmail, selectedEmployee.ReportingPerson.Person.Name));
                }

                if (selectedEmployee.Manager != null)
                {
                    emailContext.CcAddress.Add(new MailAddress(selectedEmployee.Manager.OfficialEmail, selectedEmployee.Manager.Person.Name));
                }

                if (settings.POCSettings != null)
                {
                    var selectedPOC = _userRepository.Get(settings.POCSettings.HRDepartmentLevel1, "Person");
                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTimesheetApprovalReminder(int approverId)
        {
            var emailContext = new EmailContext();

            var selectedUser = _userRepository.Get(approverId, "Person");


            // Get reportees
            var myReportees = _userRepository.GetAllBy(u => u.ReportingPersonId == approverId && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.Id).ToList();
            var pendingSheets = _timeSheetRepository.GetAllBy(r => r.State == TimeSheetState.PendingApproval && myReportees.Contains(r.CreatedByUserId), "CreatedByUser.Person").ToList();

            // Kind of Bad logic, But will work for now. So let's keep it.
            var settings = _settingsService.GetSiteSettings();
            if (settings.TimeSheetSettings?.MaxTimeSheetApprovalMisses > 0)
                {
                    var currentCount = pendingSheets.Count;
                    if (currentCount < settings.TimeSheetSettings.MaxTimeSheetApprovalMisses)
                    {
                        emailContext.DropEmail = true;
                    }
                }

            if (pendingSheets.Any())
            {
                var reportCard = new StringBuilder();
                reportCard.AppendLine("<table width='900px' bgcolor='#999999'>");

                foreach (var pendingSheet in pendingSheets)
                {
                    reportCard.AppendLine("<tr bgcolor='#ffffff'>");
                    reportCard.AppendLine($"<td>{pendingSheet.Title}</td><td>{pendingSheet.CreatedByUser.Person.Name}</td><td>{pendingSheet.Date.ToShortDateString()}</td>");
                    reportCard.AppendLine("</tr>");
                }

                reportCard.AppendLine("</table>");

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Name]", selectedUser.Person.Name),
                    new PlaceHolder("[Report]", reportCard.ToString())
                };

                emailContext.Subject = "Pending Timesheets to be Approved.";

                emailContext.ToAddress.Add(new MailAddress(selectedUser.OfficialEmail, selectedUser.Person.Name));
            }
            else
            {
                emailContext.DropEmail = true;
            }

            return emailContext;
        }             
        public EmailContext ComposeEmailContextForTimesheetSummary(int employeeId)
        {
            var emailContext = new EmailContext();

            var selectedEmployee = _userRepository.Get(employeeId, "Person,ReportingPerson.Person,Manager.Person");

            if (selectedEmployee != null)
            {
                var lastMonday = DateTime.Today.Previous(DayOfWeek.Monday);
                var lastFriday = DateTime.Today.Previous(DayOfWeek.Friday);

                var timesheetEntries =
                    _timeSheetRepository.GetAllBy(t => t.Date >= lastMonday && t.CreatedByUserId == employeeId).ToList();
                var timesheetIds = timesheetEntries.Select(t => t.Id).ToList();
                var timesheetLineItems =
                    _timeSheetLineItemRepository.GetAllBy(l => timesheetIds.Contains(l.TimeSheetId),
                        o => o.OrderBy(t => t.TimeSheet.Date), "Project,TimeSheet").ToList();

                var reportCard = new StringBuilder();
                reportCard.AppendLine("<table width='900px' bgcolor='#999999'>");
                reportCard.AppendLine("<tr bgcolor='#ffffff'>");

                while (lastMonday <= lastFriday)
                {
                    reportCard.AppendLine($"<td>{lastMonday.DayOfWeek}-{lastMonday.Date.ToShortDateString()}</td>");
                    lastMonday = lastMonday.AddDays(1);
                }
                reportCard.AppendLine("</tr>");
                reportCard.AppendLine("<tr bgcolor='#ffffff'>");

                //Reset to Monday
                lastMonday = DateTime.Today.Previous(DayOfWeek.Monday);
                while (lastMonday <= lastFriday)
                {
                    var selectedTimeSheetEntry = timesheetEntries.FirstOrDefault(e => e.Date.Date == lastMonday.Date);
                    if (selectedTimeSheetEntry != null)
                    {
                        reportCard.AppendLine($"<td>{selectedTimeSheetEntry.TotalHours} hours- {selectedTimeSheetEntry.State}</td>");
                    }
                    else
                    {
                        reportCard.AppendLine("<td>No Timesheet</td>");
                    }

                    lastMonday = lastMonday.AddDays(1);
                }
                reportCard.AppendLine("</tr>");
                reportCard.AppendLine("</table>");


                var lineDetails = new StringBuilder();
                lineDetails.AppendLine("<table width='1024px' bgcolor='#999999'>");
                lineDetails.AppendLine(
                    "<tr bgcolor='#ffffff'><td>Date</td><td>Project</td><td>Task</td><td>Hours</td><td>Billable/Non-Billable</td><td>Comments</td>");
                foreach (var timesheetLineItem in timesheetLineItems)
                {
                    lineDetails.AppendLine("<tr bgcolor='#ffffff'>");
                    lineDetails.AppendLine($"<td>{timesheetLineItem.TimeSheet.Date.ToShortDateString()}</td><td>{timesheetLineItem.Project.Title}</td><td>{timesheetLineItem.TaskSummary}</td><td>{timesheetLineItem.Effort}</td><td>{timesheetLineItem.WorkType}</td><td>{timesheetLineItem.Comments}</td>");
                    lineDetails.AppendLine("</tr>");
                }
                lineDetails.AppendLine("</table>");

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Name]", selectedEmployee.Person.Name),
                    new PlaceHolder("[TimeSheetSummary]", reportCard.ToString()),
                    new PlaceHolder("[TimeSheetDetails]", lineDetails.ToString())
                };

                emailContext.Subject = "Timesheet Summary";


                emailContext.ToAddress.Add(new MailAddress(selectedEmployee.OfficialEmail, selectedEmployee.Person.Name));

                if (selectedEmployee.ReportingPerson != null)
                {
                    emailContext.CcAddress.Add(new MailAddress(selectedEmployee.ReportingPerson.OfficialEmail, selectedEmployee.ReportingPerson.Person.Name));
                }

                if (selectedEmployee.Manager != null)
                {
                    emailContext.CcAddress.Add(new MailAddress(selectedEmployee.Manager.OfficialEmail, selectedEmployee.Manager.Person.Name));
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTimesheetSubmitted(int timeSheetId)
        {
            var emailContext = new EmailContext();

            var selectedTimeSheet = _timeSheetRepository.Get(timeSheetId, "CreatedByUser.ReportingPerson.Person,CreatedByUser.Manager.Person,CreatedByUser.Person");
            var timesheetLineItems = _timeSheetLineItemRepository.GetAllBy(l => l.TimeSheetId == selectedTimeSheet.Id, "Project,TimeSheet").ToList();

            var lineDetails = new StringBuilder();
            lineDetails.AppendLine("<table width='1024px' bgcolor='#999999'>");
            lineDetails.AppendLine("<tr bgcolor='#ffffff'><td>Project</td><td>Task</td><td>Hours</td><td>Billable/Non-Billable</td><td>Comments</td>");
            foreach (var timesheetLineItem in timesheetLineItems)
            {
                lineDetails.AppendLine("<tr bgcolor='#ffffff'>");
                lineDetails.AppendLine($"<td>{timesheetLineItem.Project.Title}</td><td>{timesheetLineItem.TaskSummary}</td><td>{timesheetLineItem.Effort}</td><td>{timesheetLineItem.WorkType}</td><td>{timesheetLineItem.Comments}</td>");
                lineDetails.AppendLine("</tr>");
            }
            lineDetails.AppendLine("</table>");

            emailContext.PlaceHolders = new List<PlaceHolder>
            {
                new PlaceHolder("[ApproverName]", selectedTimeSheet.CreatedByUser.ReportingPerson.Person.Name),
                new PlaceHolder("[Name]", selectedTimeSheet.CreatedByUser.Person.Name),
                new PlaceHolder("[Date]", selectedTimeSheet.Date.ToShortDateString()),
                new PlaceHolder("[TotalHours]", selectedTimeSheet.TotalHours.ToString(CultureInfo.InvariantCulture)),
                new PlaceHolder("[TimeSheetDetails]", lineDetails.ToString()),
                new PlaceHolder("[Id]", selectedTimeSheet.Id.ToString())
            };

            emailContext.Subject = "Timesheet Submitted by " + selectedTimeSheet.CreatedByUser.Person.Name + " for " + selectedTimeSheet.Date.ToShortDateString();
            if (selectedTimeSheet.CreatedByUser.ReportingPerson != null)
            {
                emailContext.ToAddress.Add(new MailAddress(selectedTimeSheet.CreatedByUser.ReportingPerson.OfficialEmail, selectedTimeSheet.CreatedByUser.ReportingPerson.Person.Name));
            }

            if (selectedTimeSheet.CreatedByUser.Manager != null)
            {
                emailContext.ToAddress.Add(new MailAddress(selectedTimeSheet.CreatedByUser.Manager.OfficialEmail, selectedTimeSheet.CreatedByUser.Manager.Person.Name));
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTimeSheetUpdated(int timeSheetId)
        {
            var emailContext = new EmailContext();

            var selectedTimeSheet = _timeSheetRepository.Get(timeSheetId, "CreatedByUser.ReportingPerson.Person,CreatedByUser.Manager.Person,CreatedByUser.Person");

            var timesheetLineItems = _timeSheetLineItemRepository.GetAllBy(l => l.TimeSheetId == selectedTimeSheet.Id, "Project, TimeSheet").ToList();

            var lineDetails = new StringBuilder();
            lineDetails.AppendLine("<table width='1024px' bgcolor='#999999'>");
            lineDetails.AppendLine("<tr bgcolor='#ffffff'><td>Project</td><td>Task</td><td>Hours</td><td>Billable/Non-Billable</td><td>Comments</td>");
            foreach (var timesheetLineItem in timesheetLineItems)
            {
                lineDetails.AppendLine("<tr bgcolor='#ffffff'>");
                lineDetails.AppendLine($"<td>{timesheetLineItem.Project.Title}</td><td>{timesheetLineItem.TaskSummary}</td><td>{timesheetLineItem.Effort}</td><td>{timesheetLineItem.WorkType}</td><td>{timesheetLineItem.Comments}</td>");
                lineDetails.AppendLine("</tr>");
            }
            lineDetails.AppendLine("</table>");

            emailContext.PlaceHolders = new List<PlaceHolder>
            {
                new PlaceHolder("[ApproverName]", selectedTimeSheet.CreatedByUser.ReportingPerson.Person.Name),
                new PlaceHolder("[Name]", selectedTimeSheet.CreatedByUser.Person.Name),
                new PlaceHolder("[Date]", selectedTimeSheet.Date.ToShortDateString()),
                new PlaceHolder("[TotalHours]", selectedTimeSheet.TotalHours.ToString(CultureInfo.InvariantCulture)),
                new PlaceHolder("[TimeSheetDetails]", lineDetails.ToString()),
                new PlaceHolder("[Id]", selectedTimeSheet.Id.ToString())
            };

            emailContext.Subject = "Timesheet Updated by " + selectedTimeSheet.CreatedByUser.Person.Name + " for " + selectedTimeSheet.Date.ToShortDateString();

            if (selectedTimeSheet.CreatedByUser.ReportingPerson != null)
            {
                emailContext.ToAddress.Add(new MailAddress(selectedTimeSheet.CreatedByUser.ReportingPerson.OfficialEmail, selectedTimeSheet.CreatedByUser.ReportingPerson.Person.Name));
            }

            if (selectedTimeSheet.CreatedByUser.Manager != null)
            {
                emailContext.ToAddress.Add(new MailAddress(selectedTimeSheet.CreatedByUser.Manager.OfficialEmail, selectedTimeSheet.CreatedByUser.Manager.Person.Name));
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTimeSheetApproved(int timeSheetId)
        {
            var emailContext = new EmailContext();

            var selectedTimeSheet = _timeSheetRepository.Get(timeSheetId, "CreatedByUser.ReportingPerson.Person,CreatedByUser.Person");
            emailContext.PlaceHolders = new List<PlaceHolder>
            {
                new PlaceHolder("[Name]", selectedTimeSheet.CreatedByUser.Person.Name),
                new PlaceHolder("[Date]", selectedTimeSheet.Date.ToShortDateString()),
                new PlaceHolder("[TotalHours]", selectedTimeSheet.TotalHours.ToString(CultureInfo.InvariantCulture)),
                new PlaceHolder("[ApproverName]", selectedTimeSheet.CreatedByUser.ReportingPerson.Person.Name),
                new PlaceHolder("[ApproverComments]", selectedTimeSheet.ApproverComments),
                new PlaceHolder("[Id]", selectedTimeSheet.Id.ToString())
            };

            emailContext.Subject = $"Your Timesheet for {selectedTimeSheet.Date.ToShortDateString()} is {"Approved"}";

            emailContext.ToAddress.Add(new MailAddress(selectedTimeSheet.CreatedByUser.OfficialEmail, selectedTimeSheet.CreatedByUser.Person.Name));

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTimeSheetNeedsCorrection(int timeSheetId)
        {
            var emailContext = new EmailContext();

            var selectedTimeSheet = _timeSheetRepository.Get(timeSheetId, "CreatedByUser.ReportingPerson.Person,CreatedByUser.Person");
            emailContext.PlaceHolders = new List<PlaceHolder>
            {
                new PlaceHolder("[Name]", selectedTimeSheet.CreatedByUser.Person.Name),
                new PlaceHolder("[Date]", selectedTimeSheet.Date.ToShortDateString()),
                new PlaceHolder("[TotalHours]", selectedTimeSheet.TotalHours.ToString(CultureInfo.InvariantCulture)),
                new PlaceHolder("[ApproverName]", selectedTimeSheet.CreatedByUser.ReportingPerson.Person.Name),
                new PlaceHolder("[ApproverComments]", selectedTimeSheet.ApproverComments),
                new PlaceHolder("[Id]", selectedTimeSheet.Id.ToString())
            };

            emailContext.Subject = $"Your Timesheet for {selectedTimeSheet.Date.ToShortDateString()} {"Needs Correction"}";

            emailContext.ToAddress.Add(new MailAddress(selectedTimeSheet.CreatedByUser.OfficialEmail, selectedTimeSheet.CreatedByUser.Person.Name));

            return emailContext;
        }
    }
}
