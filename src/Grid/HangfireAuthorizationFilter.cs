using Hangfire.Dashboard;
using Microsoft.Owin;

namespace Grid
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {

            var environment = context.GetOwinEnvironment();
            var owinContext = new OwinContext(environment);
            return owinContext.Authentication.User.Identity.IsAuthenticated;
        }
    }
}