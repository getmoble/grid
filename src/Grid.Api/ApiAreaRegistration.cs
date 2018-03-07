using System.Web.Mvc;

namespace Grid.Api
{
    public class ApiAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Api";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Api_default",
                "Api/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Grid.Api.Controllers" }
            );
        }
    }
}