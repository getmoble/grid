using System.Web.Mvc;

namespace Grid.Areas.Company
{
    public class CompanyAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Company";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Company_default",
                "Company/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}