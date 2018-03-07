using System.Web.Mvc;

namespace Grid.Areas.Templates
{
    public class TemplatesAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Templates";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Templates_default2",
                "Templates/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}