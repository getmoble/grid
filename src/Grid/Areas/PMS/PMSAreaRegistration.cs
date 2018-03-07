using System.Web.Mvc;

namespace Grid.Areas.PMS
{
    public class PMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "PMS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PMS_default",
                "PMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Grid.Areas.PMS.Controllers" }
            );
        }
    }
}