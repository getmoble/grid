using System.Web.Mvc;

namespace Grid.Generic.Api
{
    public class GenericApiAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "GenericApi";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GenericApi_default",
                "GenericApi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Grid.Generic.Api.Controllers" }
            );
        }
    }
}