using System.Web.Mvc;
using System.Web.Routing;
using Grid.Infrastructure;

namespace Grid.UI.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new { Constraint = new GridRouteConstraint() }, new[] { "Grid.UI.WebApp.Controllers" });
        }
    }
}
