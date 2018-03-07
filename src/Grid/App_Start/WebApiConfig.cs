using System.Web.Http;

namespace Grid.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "generic",
                routeTemplate: "generic/{controller}/{id}/{searchParams}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}