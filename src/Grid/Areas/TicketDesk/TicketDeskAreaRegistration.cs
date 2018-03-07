using System.Web.Mvc;

namespace Grid.Areas.TicketDesk
{
    public class TicketDeskAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "TicketDesk";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TicketDesk_default",
                "TicketDesk/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}