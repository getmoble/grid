using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.CRM
{
    public class CRMBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "CRM";
        }
    }
}