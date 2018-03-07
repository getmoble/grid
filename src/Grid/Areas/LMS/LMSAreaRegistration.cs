using System.Web.Mvc;

namespace Grid.Areas.LMS
{
    public class LMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "LMS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LMS_default",
                "LMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}