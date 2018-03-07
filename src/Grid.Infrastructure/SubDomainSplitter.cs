using System;

namespace Grid.Infrastructure
{
    public class SubDomainSplitter
    {
        public static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                var nodes = host.Split('.');
                string subDomain = "";
                int startNode = 0;
                if (nodes.Length >= 1)
                {
                    if (nodes[0] == "www")
                    {
                        startNode = 1;
                        subDomain = nodes[startNode];
                        return subDomain;
                    }

                    subDomain = nodes[startNode];
                    return subDomain;
                }

                if (nodes.Length == 1)
                {
                    subDomain = nodes[startNode];
                    return subDomain;
                }

                return null;
            }

            return null;
        }
    }
}