using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.Company
{
    public class CompanyBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "Company";
        }
    }
}