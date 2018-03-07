using System.Web.Mvc;

namespace Grid.Infrastructure.Filters
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Depth, User-Agent, X-File-Size, X-Requested-With, If-Modified-Since, X-File-Name, Cache-Control");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            base.OnActionExecuting(filterContext);
        }
    }
}
