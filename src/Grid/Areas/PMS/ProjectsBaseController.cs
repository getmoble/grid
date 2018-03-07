using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.PMS
{
    public class ProjectsBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "Projects";
        }
    }
}