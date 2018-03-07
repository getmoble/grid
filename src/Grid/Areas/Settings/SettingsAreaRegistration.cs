using System.Web.Mvc;

namespace Grid.Areas.Settings
{
    public class SettingsAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Settings";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Settings_default",
                "Settings/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}