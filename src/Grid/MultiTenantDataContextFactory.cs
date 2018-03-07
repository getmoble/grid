using System;
using Autofac;
using Autofac.Multitenant;
using Elmah;
using Grid.Infrastructure;
using Grid.Data;

namespace Grid
{
    public static class MultiTenantDataContextFactory
    {
        public static GridDataContext Create(IComponentContext c)
        {
            var strategy = c.Resolve<ITenantIdentificationStrategy>();
            object tenantId;
            strategy.TryIdentifyTenant(out tenantId);

            var cacheKey = $"TKey:{tenantId}";
            var connectionString = HttpCacheWrapper.GetFromSession<string>(cacheKey);

            // If it's empty, get it from database
            if (string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    // Fetch from database
                    TenantDataFetcher.GetConnectionString(tenantId, out connectionString);

                    // Let's cache it, it it's valid
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        HttpCacheWrapper.SetInSession(cacheKey, connectionString);
                    }
                    
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }

            // Create the instance with the connection string and return
            return new GridDataContext(connectionString);
        }
    }
}