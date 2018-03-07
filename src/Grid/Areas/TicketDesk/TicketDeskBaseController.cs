using System.Web.Mvc;
using Grid.Infrastructure;

namespace Grid.Areas.TicketDesk
{
    public class TicketDeskBaseController: GridBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApplicationTitle = "Ticket Desk";
        }
    }
}