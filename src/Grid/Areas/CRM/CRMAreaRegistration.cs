using System.Web.Mvc;

namespace Grid.Areas.CRM
{
    public class CRMAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "CRM";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CRM_default",
                "CRM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}