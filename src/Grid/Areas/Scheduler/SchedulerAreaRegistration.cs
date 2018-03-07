using System.Web.Mvc;

namespace Grid.Areas.Scheduler
{
    public class SchedulerAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Scheduler";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Scheduler_default",
                "Scheduler/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}