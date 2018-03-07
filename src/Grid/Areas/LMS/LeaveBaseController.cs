using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.LMS
{
    public class LeaveBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "Leave";
        }
    }
}