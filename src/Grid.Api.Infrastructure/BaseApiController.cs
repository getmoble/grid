using System;
using System.Web.Mvc;

namespace Grid.Api.Infrastructure
{
    public class BaseApiController : Controller
    {
        public virtual ActionResult ThrowIfNotLoggedIn(Func<ActionResult> action)
        {
            var loggedIn = User.Identity.IsAuthenticated;
            if (loggedIn)
            {
                return action();
            }

            Response.StatusCode = 403;
            Response.StatusDescription = "Access Denied";
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public virtual ActionResult TryExecute<T>(Func<T> operation,
            JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            try
            {
                var result = operation();
                var apiResult = new ApiResult<T>
                {
                    Status = true,
                    Message = "Success",
                    Result = result
                };

                return Json(apiResult, behavior);
            }
            catch (Exception ex)
            {

                var apiResult = new ApiResult<T>
                {
                    Status = false,
                    Message = "Oops... Something bad has happened..."
                };

                return Json(apiResult, behavior);
            }
        }
    }
}