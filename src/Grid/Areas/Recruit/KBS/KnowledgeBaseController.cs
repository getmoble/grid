using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.KBS
{
    public class KnowledgeBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "Knowledge Base";
        }
    }
}