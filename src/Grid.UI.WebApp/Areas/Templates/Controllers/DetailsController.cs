using System.Web.Mvc;

namespace Grid.UI.WebApp.Areas.Templates.Controllers
{
    public class DetailsController : Controller
    {
        public ActionResult Index(string type)
        {
            switch (type)
            {
                case "Tickets":
                    return PartialView("_Ticket");
                case "Candidates":
                    return PartialView("_Candidates");
            }

            return PartialView("_Bootstrap");
        }
    }
}