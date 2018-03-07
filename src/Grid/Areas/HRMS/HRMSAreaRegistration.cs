using System.Web.Mvc;

namespace Grid.Areas.HRMS
{
    public class HRMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "HRMS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HRMS_default",
                "HRMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}