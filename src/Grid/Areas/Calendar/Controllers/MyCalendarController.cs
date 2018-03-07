using System.Web.Mvc;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.Calendar.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class MyCalendarController : GridBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}