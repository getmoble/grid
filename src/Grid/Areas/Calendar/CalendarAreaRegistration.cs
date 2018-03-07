using System.Web.Mvc;

namespace Grid.Areas.Calendar
{
    public class CalendarAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Calendar";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Calendar_default",
                "Calendar/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}