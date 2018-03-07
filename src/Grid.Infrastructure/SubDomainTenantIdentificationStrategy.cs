using System.Web;
using Autofac.Multitenant;

namespace Grid.Infrastructure
{
    public class SubDomainTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public bool TryIdentifyTenant(out object tenantId)
        {
            tenantId = null;
            try
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var subdomain = SubDomainSplitter.GetSubDomain(context.Request.Url);
                    // Use this subdomain to get the tenantId from the database. As of now just return subdomain.

                    /*
                    string tenantConnectionString = ConfigurationManager.ConnectionStrings["TenantManagement"].ConnectionString;
                    var sqlConnection = new SqlConnection(tenantConnectionString);
                    var sqlCommand = new SqlCommand("select connectionstring from Tenants where SubDomain=@subdomain", sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@subdomain", subdomain);
                    var connectionString = sqlCommand.ExecuteScalar().ToString();
                    */

                    tenantId = subdomain;
                }
            }
            catch (HttpException)
            {
                // Happens at app startup in IIS 7.0
            }

            return tenantId != null;
        }

        //public string GetConnectionStringOfTenant(out object tenantId)
        //{
        //    var connStr = "";
        //    TryIdentifyTenant(out tenantId);
        //    var connectionString = TenantDataFetcher.GetConnectionString(tenantId, out connStr);
        //    return connStr;
        //}
    }
}