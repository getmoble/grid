using System.Web.Mvc;
using Grid.Models;

namespace Grid.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotAuthorized()
        {
            return View();
        }

        public ActionResult Oops(int id)
        {
            var errorModel = new ErrorModel();
            switch (id)
            {
                case 404:
                    Response.StatusCode = 404;
                    errorModel.StatusCode = 404;
                    errorModel.Message = "Oops, Grid couldn't find that";
                    return View(errorModel);
                case 500:
                    Response.StatusCode = 500;
                    errorModel.StatusCode = 500;
                    errorModel.Message = "Oops, Something got crashed";
                    return View(errorModel);
                default:
                    Response.StatusCode = 500;
                    errorModel.StatusCode = 500;
                    errorModel.Message = "I don't know what happened";
                    return View(errorModel);
            }
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}