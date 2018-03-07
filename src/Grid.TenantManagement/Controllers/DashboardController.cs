using System.Web.Mvc;

namespace Grid.TenantManagement.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}