using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Services.Interfaces;
using Grid.Providers.Email;
using Grid.Features.Settings.Services.Interfaces;

namespace Grid.Features.Recruit.Services
{
    public class InterviewRoundService: IInterviewRoundService
    {
        private readonly ISettingsService _settingsService;
        private readonly IUserRepository _userRepository;
        private readonly IInterviewRoundRepository _interviewRoundRepository;

        public InterviewRoundService(ISettingsService settingsService,
                                     IUserRepository userRepository,
                                     IInterviewRoundRepository interviewRoundRepository)
        {
            _settingsService = settingsService;
            _userRepository = userRepository;
            _interviewRoundRepository = interviewRoundRepository;
        }

        public EmailContext ComposeEmailContextForInterviewScheduled(int interviewId)
        {
            var emailContext = new EmailContext();
            var selectedInterview = _interviewRoundRepository.Get(interviewId, "JobOpening,Candidate.Person,Round,Interviewer.Person");
            if (selectedInterview != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Id]", selectedInterview.Id.ToString()),
                    new PlaceHolder("[JobOpening]", selectedInterview.JobOpening.Title),
                    new PlaceHolder("[Candidate]", selectedInterview.Candidate.Person.Name),
                    new PlaceHolder("[Round]", selectedInterview.Round.Title),
                    new PlaceHolder("[Interviewer]", selectedInterview.Interviewer.Person.Name),
                    new PlaceHolder("[ScheduledOn]", selectedInterview.ScheduledOn.ToString()),
                };

                emailContext.Subject = "Interview Scheduled";

                emailContext.ToAddress.Add(new MailAddress(selectedInterview.Interviewer.OfficialEmail, selectedInterview.Interviewer.Person.Name));

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    var selectedPOC = _userRepository.Get(settings.POCSettings.HRDepartmentLevel1, "Person");
                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }

                if (selectedInterview.ScheduledOn.HasValue)
                {
                    var description = $"{selectedInterview.Round.Title} for {selectedInterview.Candidate.Person.Name} on {selectedInterview.ScheduledOn}";
                    var icsFileContent = IcsHelper.Generate(selectedInterview.ScheduledOn.Value, "Interview Scheduled", description);
                    var emailAttachment = new EmailAttachment()
                    {
                        FileName = "Interview.ics",
                        FileType = "text/ics",
                        Content = Encoding.ASCII.GetBytes(icsFileContent)
                    };

                    emailContext.EmailAttachments.Add(emailAttachment);
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForInterviewReminder(int interviewId)
        {
            var emailContext = new EmailContext();
            var selectedInterview = _interviewRoundRepository.Get(interviewId, "JobOpening,Candidate.Person,Round,Interviewer.Person");
            if (selectedInterview != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Id]", selectedInterview.Id.ToString()),
                    new PlaceHolder("[JobOpening]", selectedInterview.JobOpening.Title),
                    new PlaceHolder("[Candidate]", selectedInterview.Candidate.Person.Name),
                    new PlaceHolder("[Round]", selectedInterview.Round.Title),
                    new PlaceHolder("[Interviewer]", selectedInterview.Interviewer.Person.Name),
                    new PlaceHolder("[ScheduledOn]", selectedInterview.ScheduledOn.ToString()),
                };

                emailContext.Subject = "Interview Reminder";

                emailContext.ToAddress.Add(new MailAddress(selectedInterview.Interviewer.OfficialEmail, selectedInterview.Interviewer.Person.Name));
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForInterviewFeedbackReminder(int interviewId)
        {
            var emailContext = new EmailContext();
            var selectedInterview = _interviewRoundRepository.Get(interviewId, "JobOpening,Candidate.Person,Round,Interviewer.Person");
            if (selectedInterview != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Id]", selectedInterview.Id.ToString()),
                    new PlaceHolder("[JobOpening]", selectedInterview.JobOpening.Title),
                    new PlaceHolder("[Candidate]", selectedInterview.Candidate.Person.Name),
                    new PlaceHolder("[Round]", selectedInterview.Round.Title),
                    new PlaceHolder("[Interviewer]", selectedInterview.Interviewer.Person.Name),
                    new PlaceHolder("[ScheduledOn]", selectedInterview.ScheduledOn.ToString()),
                };

                emailContext.Subject = "Interview Feedback Reminder";

                emailContext.ToAddress.Add(new MailAddress(selectedInterview.Interviewer.OfficialEmail, selectedInterview.Interviewer.Person.Name));

                var settings = _settingsService.GetSiteSettings();
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
    }
}
