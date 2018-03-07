using System.Web.Mvc;

namespace Grid.Areas.Accounting
{
    public class AccountingAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Accounting";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Accounting_default",
                "Accounting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}