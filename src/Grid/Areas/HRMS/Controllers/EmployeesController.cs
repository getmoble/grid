using Grid.Infrastructure;
using Grid.Infrastructure.Filters;
using System.Web.Mvc;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 215)]
    public class EmployeesController : GridBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}