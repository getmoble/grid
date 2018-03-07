using System.Web.Mvc;

namespace Grid.Areas.KBS
{
    public class KBSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "KBS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KBS_default",
                "KBS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}