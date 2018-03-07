using System.Web.Mvc;

namespace Grid.Areas.Payroll
{
    public class PayrollAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Payroll";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Payroll_default",
                "Payroll/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}