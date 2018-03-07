using System.Web.Mvc;
using Grid.Features.EmailService;

namespace Grid.Areas.Scheduler.Controllers
{
    public class EventReminderController : Controller
    {
        private readonly EmailComposerService _emailComposerService;
        public EventReminderController(EmailComposerService emailComposerService)
        {
            _emailComposerService = emailComposerService;
        }

        public ActionResult BirthdayAnniversary()
        {
            #if !DEBUG
                _emailComposerService.SendBirthdayAndAnniversaryReminder();
            #endif

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}