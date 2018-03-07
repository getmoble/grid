using System.Web.Mvc;

namespace Grid.TenantManagement.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.App = "Grid";
            return View();
        }
    }
}