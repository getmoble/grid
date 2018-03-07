using System;
using System.Web.Mvc;
using Grid.Features.EmailService;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Areas.Scheduler.Controllers
{
    public class InterviewReminderController: Controller
    {
        private readonly IInterviewRoundRepository _interviewRoundRepository;
        private readonly EmailComposerService _emailComposerService;
        public InterviewReminderController(IInterviewRoundRepository interviewRoundRepository,
                                      EmailComposerService emailComposerService)
        {
            _interviewRoundRepository = interviewRoundRepository;
            _emailComposerService = emailComposerService;
        }

        public ActionResult Today()
        {
            var today = DateTime.UtcNow.Date;
            var todaysInterviews = _interviewRoundRepository.GetAllBy(t => t.Status == InterviewStatus.Scheduled && System.Data.Entity.DbFunctions.TruncateTime(t.ScheduledOn) == today);
            foreach (var interview in todaysInterviews)
            {
                var interviewId = interview.Id;
                
                #if !DEBUG
                    _emailComposerService.InterviewReminder(interviewId);
                #endif
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Feedback()
        {
            var yesterday = DateTime.UtcNow.Date.AddDays(-1);
            var pendingInterviews = _interviewRoundRepository.GetAllBy(t => t.Status == InterviewStatus.Scheduled & t.ScheduledOn < yesterday);
            foreach (var interview in pendingInterviews)
            {
                var interviewId = interview.Id;

                #if !DEBUG
                    _emailComposerService.InterviewFeedbackReminder(interviewId);
                #endif
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}