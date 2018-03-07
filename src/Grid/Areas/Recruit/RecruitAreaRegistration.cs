using System.Web.Mvc;

namespace Grid.Areas.Recruit
{
    public class RecruitAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Recruit";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Recruit_default",
                "Recruit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}