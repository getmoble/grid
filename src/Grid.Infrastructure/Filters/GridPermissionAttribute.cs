using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Grid.Infrastructure.Filters
{
    public class GridPermissionAttribute : ActionFilterAttribute
    {
        public int PermissionCode { get; set; }
        public int[] PermissonList { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var loggedInUser = filterContext.HttpContext.User as Principal;
            if (loggedInUser != null)
            {
                // First check for Permission Code, Most case we will use a single Permission
                if (!PermissionChecker.CheckPermission(loggedInUser.Permissions, PermissionCode))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
                    { 
                        { "controller", "Error" }, 
                        { "action", "NotAuthorized" },
                        {"area", ""}
                    });
                }

                // Fallback, Check for multiple Permissions
                var counter = 0;
                if (PermissonList != null && PermissonList.Length > 0)
                {
                    counter += PermissonList.Count(selectedPermissionCode => PermissionChecker.CheckPermission(loggedInUser.Permissions, selectedPermissionCode));

                    if (counter <= 0)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
                        { 
                            { "controller", "Error" }, 
                            { "action", "NotAuthorized" },
                            {"area", ""}
                        });
                    }
                }
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}