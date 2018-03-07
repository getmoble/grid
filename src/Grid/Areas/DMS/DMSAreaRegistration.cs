using System.Web.Mvc;

namespace Grid.Areas.DMS
{
    public class DMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "DMS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DMS_default",
                "DMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}