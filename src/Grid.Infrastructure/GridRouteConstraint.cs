using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace Grid.Infrastructure
{
    public class GridRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var key = "";
            var subdomain = SubDomainSplitter.GetSubDomain(httpContext.Request.Url);
            var tenants = HttpCacheWrapper.GetFromSession<List<string>>(key);
            if (tenants != null && tenants.Contains(subdomain))
            {
                return true;
            }
            else
            {
                {
                    tenants = TenantDataFetcher.FetchTenantList();
                    HttpCacheWrapper.SetInSession(key, tenants);
                    return tenants.Contains(subdomain);
                }
            }
        }
    }
}