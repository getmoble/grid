using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.Recruit
{
    public class RecruitBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "Recruitment";
        }
    }
}