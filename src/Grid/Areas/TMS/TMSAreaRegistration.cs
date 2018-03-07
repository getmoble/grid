using System.Web.Mvc;

namespace Grid.Areas.TMS
{
    public class TMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "TMS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TMS_default",
                "TMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}