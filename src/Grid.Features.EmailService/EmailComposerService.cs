using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Services.Interfaces;
using Grid.Features.IMS.Services.Interfaces;
using Grid.Features.LMS.Services.Interfaces;
using Grid.Features.PMS.Services.Interfaces;
using Grid.Features.Recruit.Services.Interfaces;
using Grid.Features.RMS.Services.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Features.TicketDesk.Services.Interfaces;
using Grid.Providers.Email;
using Grid.Features.LMS.Entities;

namespace Grid.Features.EmailService
{
    public class EmailComposerService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly ISettingsService _settingsService;

        private readonly IRequirementService _requirementService;
        private readonly ITimeSheetService _timeSheetService;
        private readonly ILeaveService _leaveService;
        private readonly IUserService _userService;
        private readonly IAssetService _assetService;
        private readonly ITicketService _ticketService;
        private readonly ITaskService _taskService;
        private readonly IInterviewRoundService _interviewRoundService;

        public EmailComposerService(IEmailTemplateRepository emailTemplateRepository,
                                    ISettingsService settingsService,
                                    ILeaveService leaveService,
                                    IUserService userService,
                                    IAssetService assetService,
                                    IRequirementService requirementService,
                                    ITicketService ticketService,
                                    ITaskService taskService,
                                    IInterviewRoundService interviewRoundService,
                                    ITimeSheetService timeSheetService)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _settingsService = settingsService;
            _leaveService = leaveService;
            _userService = userService;
            _assetService = assetService;
            _ticketService = ticketService;
            _interviewRoundService = interviewRoundService;
            _taskService = taskService;

            _requirementService = requirementService;
            _timeSheetService = timeSheetService;
        }

        private Providers.Email.EmailService CreateEmailService()
        {
            var siteSettings = _settingsService.GetSiteSettings();
            var emailService = new Providers.Email.EmailService(siteSettings.EmailSettings);
            return emailService;
        }

        private void SendEmail(string templateName, EmailContext context)
        {
            if (context != null)
            {
                // short Circuit, If there is DropEmail set, we dont have to send this email.
                if (context.DropEmail)
                {
                    return;
                }

                var emailService = CreateEmailService();
                var emailTemplate = GetEmailTemplate(templateName);
                var emailBody = SimpleTemplateProcessor.Process(emailTemplate, context.PlaceHolders);

                var fromAddress = GetFromAddress();

                var mailMessage = new MailMessage
                {
                    From = fromAddress
                };

                // Add To address
                foreach (var toAddress in context.ToAddress)
                {
                    mailMessage.To.Add(toAddress);
                }

                // Add cc address
                foreach (var ccAddress in context.CcAddress)
                {
                    mailMessage.CC.Add(ccAddress);
                }

                // Set Subject
                mailMessage.Subject = context.Subject;

                // Set Body 
                mailMessage.Body = emailBody;
                mailMessage.IsBodyHtml = true;

                if (context.EmailAttachments.Any())
                {
                    foreach (var emailAttachment in context.EmailAttachments)
                    {
                        Stream stream = new MemoryStream(emailAttachment.Content);
                        mailMessage.Attachments.Add(new Attachment(stream, emailAttachment.FileName, emailAttachment.FileType));
                    }
                }

                emailService.SendEmail(mailMessage);
            }
        }

        private string GetEmailTemplate(string templateName)
        {
            var templateContent = string.Empty;

            var emailTemplate = _emailTemplateRepository.GetBy(e => e.Name == templateName);
            if(!string.IsNullOrEmpty(emailTemplate?.Content))
                templateContent = emailTemplate.Content;

            return templateContent;
        }

        private MailAddress GetFromAddress()
        {
            var siteSettings = _settingsService.GetSiteSettings();
            var fromAddress = new MailAddress(siteSettings.EmailSettings.FromEmail, siteSettings.EmailSettings.FromName);
            return fromAddress;
        }

        #region Requirements
        public void SendNewRequirementEmail(int requirementId)
        {
            var context = _requirementService.ComposeEmailContextForNewRequirement(requirementId);
            SendEmail("NewRequirement", context);
        }

        public void SendRequirementUpdateEmail(int requirementActivityId)
        {
            var context = _requirementService.ComposeEmailContextForRequirementUpdate(requirementActivityId);
            SendEmail("RequirementActivity", context);
        }
        #endregion

        #region TimeSheets
        public void SendReminderEmail(int employeeId, DateTime date)
        {
            var context = _timeSheetService.ComposeEmailContextForTimesheetReminder(employeeId, date);
            SendEmail("TimeSheetReminder", context);
        }

        public void SendMissedEmail(int employeeId, DateTime date)
        {
            var context = _timeSheetService.ComposeEmailContextForTimesheetMissed(employeeId, date);
            SendEmail("TimeSheetMissed", context);
        }

        public void SendApprovalReminderEmail(int approverId)
        {
            var context = _timeSheetService.ComposeEmailContextForTimesheetApprovalReminder(approverId);
            SendEmail("TimeSheetApprovalReminder", context);
        }

        public void SummaryEmail(int employeeId)
        {
            var context = _timeSheetService.ComposeEmailContextForTimesheetSummary(employeeId);
            SendEmail("TimeSheetSummary", context);
        }

        public void TimeSheetSubmitted(int timeSheetId)
        {
            var context = _timeSheetService.ComposeEmailContextForTimesheetSubmitted(timeSheetId);
            SendEmail("TimeSheetDetails", context);
        }

        public void TimeSheetUpdated(int timeSheetId)
        {
            var context = _timeSheetService.ComposeEmailContextForTimeSheetUpdated(timeSheetId);
            SendEmail("TimeSheetDetails", context);
        }

        public void TimeSheetApproved(int timeSheetId)
        {
            var context = _timeSheetService.ComposeEmailContextForTimeSheetApproved(timeSheetId);
            SendEmail("TimeSheetApproved", context);
        }

        public void TimeSheetNeedsCorrection(int timeSheetId)
        {
            var context = _timeSheetService.ComposeEmailContextForTimeSheetNeedsCorrection(timeSheetId);
            SendEmail("TimeSheetNeedsCorrection", context);
        }
        #endregion

        #region Leaves
        public void LeaveApplicationEmail(int leaveId)
        {
            var context = _leaveService.ComposeEmailContextForLeaveApplication(leaveId);
            SendEmail("LeaveApplication", context);
        }
        public void TestEmailSend(Leave leave)
        {
            var context = _leaveService.TestEmail(leave);
            SendEmail("LeaveApplication", context);
        }

        public void LeaveApprovalEmail(int leaveId, bool autoapproved)
        {
            var context = _leaveService.ComposeEmailContextForLeaveApproval(leaveId, autoapproved);
            SendEmail("LeaveApproval", context);
        }

        public void LeaveRejectedEmail(int leaveId)
        {
            var context = _leaveService.ComposeEmailContextForLeaveRejected(leaveId);
            SendEmail("LeaveRejected", context);
        }

        public void AutoApproveLeaves()
        {

        }
        #endregion

        #region BirthdayList
        public void SendBirthdayAndAnniversaryReminder()
        {
            var context = _userService.ComposeEmailContextForBirthdayReminder();
            SendEmail("BirthdayAndAnniversaryReminder", context);
        }
        #endregion

        #region IMS
        public void AssetStateChanged(int assetId)
        {
            var context = _assetService.ComposeEmailContextForAssetStateChanged(assetId);
            SendEmail("AssetStateChanged", context);
        }
        #endregion

        #region Ticket Desk
        public void TicketCreated(int ticketId)
        {
            var context = _ticketService.ComposeEmailContextForTicketCreated(ticketId);
            SendEmail("TicketCreated", context);
        }
        public void TicketUpdated(int ticketId)
        {
            var context = _ticketService.ComposeEmailContextForTicketUpdated(ticketId);
            SendEmail("TicketStatusUpdated", context);
        }
        public void TicketMissed(int ticketId)
        {
            var context = _ticketService.ComposeEmailContextForTicketMissed(ticketId);
            SendEmail("TicketMissed", context);
        }
        #endregion

        #region Tasks
        public void TaskCreated(int taskId)
        {
            var context = _taskService.ComposeEmailContextForTaskCreated(taskId);
            SendEmail("TaskCreated", context);
        }
        public void TaskUpdated(int taskId)
        {
            var context = _taskService.ComposeEmailContextForTaskUpdated(taskId);
            SendEmail("TaskUpdated", context);
        }
        public void TaskMissed(int taskId)
        {
            var context = _taskService.ComposeEmailContextForTaskMissed(taskId);
            SendEmail("TaskMissed", context);
        }
        #endregion

        #region Recruit
        public void InterviewScheduled(int interviewId)
        {
            var context = _interviewRoundService.ComposeEmailContextForInterviewScheduled(interviewId);
            SendEmail("InterviewScheduled", context);
        }
        public void InterviewReminder(int interviewId)
        {
            var context = _interviewRoundService.ComposeEmailContextForInterviewReminder(interviewId);
            SendEmail("InterviewReminder", context);
        }
        public void InterviewFeedbackReminder(int interviewId)
        {
            var context = _interviewRoundService.ComposeEmailContextForInterviewFeedbackReminder(interviewId);
            SendEmail("InterviewFeedbackReminder", context);
        }
        #endregion
    }
}
