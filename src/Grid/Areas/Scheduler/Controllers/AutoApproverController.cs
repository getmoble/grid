using System.Web.Mvc;
using Grid.Features.EmailService;
using Grid.Features.LMS.Services.Interfaces;

namespace Grid.Areas.Scheduler.Controllers
{
    public class AutoApproverController : Controller
    {
        private readonly ILeaveService _leaveService;
        private readonly EmailComposerService _emailComposerService;

        public AutoApproverController(ILeaveService leaveService,
                                      EmailComposerService emailComposerService)
        {
            _leaveService = leaveService;
            _emailComposerService = emailComposerService;
        }

        public ActionResult TimeSheet()
        {
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Leave()
        {
            var pendingLeaves = _leaveService.GetPendingLeaves();
            foreach (var pendingLeave in pendingLeaves)
            {
                var status = _leaveService.Approve(pendingLeave.Id, 1, "Auto Approved by Grid-Bot", true);
                if (status)
                {
                    #if !DEBUG
                        _emailComposerService.LeaveApprovalEmail(pendingLeave.Id, true);
                    #endif
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}