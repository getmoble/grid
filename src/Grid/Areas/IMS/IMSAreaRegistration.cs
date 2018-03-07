using System.Web.Mvc;

namespace Grid.Areas.IMS
{
    public class IMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "IMS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "IMS_default",
                "IMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}