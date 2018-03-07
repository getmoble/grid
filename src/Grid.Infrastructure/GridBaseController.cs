using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Grid.Infrastructure
{
    public class GridBaseController : Controller
    {
        public Principal WebUser => User as Principal;

        // Public Methods
        public ActionResult ExecuteIfValid(Func<ActionResult> onValid, Func<ActionResult> onInvalid)
        {
            return ModelState.IsValid ? onValid() : onInvalid();
        }

        public ActionResult RedirectIfNotLoggedIn(Func<ActionResult> action)
        {
            //var collection = this.Request.Headers;
            return User.Identity.IsAuthenticated ? action() : new RedirectResult(FormsAuthentication.LoginUrl + "?returnUrl=" + Server.UrlEncode(Request.RawUrl));
        }
        public ActionResult HandleIfOperationFailed(Func<ActionResult> action, Func<Exception, ActionResult> onError)
        {
            try
            {
                return action();
            }
            catch (Exception exception)
            {
                return onError(exception);
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ApplicationTitle = "Grid";

            base.OnActionExecuting(filterContext);
            if (WebUser != null)
            {
                ViewBag.Permissions = WebUser.Permissions;
                ViewBag.Name = WebUser.Name;
            }
        }

        public ActionResult CheckForNullAndExecute<T>(T payload, Func<ActionResult> operation)
        {
            return payload == null ? HttpNotFound() : operation();
        }
    }
}