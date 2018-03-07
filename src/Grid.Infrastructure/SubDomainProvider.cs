using System;
using System.Web;

namespace Grid.Infrastructure
{
    public class SubDomainProvider : ISubDomainProvider
    {
        public string GetSubDomain()
        {
            var result = "localhost";
            var host = HttpContext.Current.Request.Url.Host; // not checked (off the top of my head
            if (host.Split('.').Length > 1)
            {
                var index = host.IndexOf(".", StringComparison.Ordinal);
                var subdomain = host.Substring(0, index);
                if (subdomain != "www")
                {
                    result = subdomain;
                }
            }

            return result;
        }
    }
}
