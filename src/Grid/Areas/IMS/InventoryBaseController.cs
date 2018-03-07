using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.IMS
{
    public class InventoryBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "Inventory";
        }
    }
}