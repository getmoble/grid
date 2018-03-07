using System.Web.Mvc;

namespace Grid.Areas.RMS
{
    public class RMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "RMS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RMS_default",
                "RMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Grid.Areas.RMS.Controllers" }
            );
        }
    }
}