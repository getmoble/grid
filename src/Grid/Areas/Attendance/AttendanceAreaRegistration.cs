using System.Web.Mvc;

namespace Grid.Areas.Attendance
{
    public class AttendanceAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Attendance";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Attendance_default",
                "Attendance/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}