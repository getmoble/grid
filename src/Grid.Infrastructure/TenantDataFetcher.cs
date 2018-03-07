using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Elmah;

namespace Grid.Infrastructure
{
    public class TenantDataFetcher
    {
        public static bool GetConnectionString(object tenantId, out string connStr)
        {
            connStr = null;
            try
            {
                string tenantConnectionString = ConfigurationManager.ConnectionStrings["Tenants"].ConnectionString;
                var sqlConnection = new SqlConnection(tenantConnectionString);
                var sqlCommand = new SqlCommand("select connectionstring from Tenants where SubDomain=@subdomain", sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@subdomain", tenantId);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    connStr = reader["connectionstring"].ToString();
                }
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        public static bool GetConnectionStringAndAnalyticsId(object tenantId, out string connStr, out string googleAnalyticsId)
        {
            connStr = googleAnalyticsId = null;
            try
            {
                string tenantConnectionString = ConfigurationManager.ConnectionStrings["Tenants"].ConnectionString;
                var sqlConnection = new SqlConnection(tenantConnectionString);
                var sqlCommand = new SqlCommand("select connectionstring from Tenants where SubDomain=@subdomain", sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@subdomain", tenantId);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    connStr = reader["connectionstring"].ToString();
                    googleAnalyticsId = "yettobedone";
                }
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        public static List<string> FetchTenantList()
        {
            var tenantsList = new List<string>();
            try
            {
                string tenantConnectionString = ConfigurationManager.ConnectionStrings["Tenants"].ConnectionString;
                var sqlConnection = new SqlConnection(tenantConnectionString);
                var sqlCommand = new SqlCommand("select SubDomain from Tenants", sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                int index = 0;
                while (reader.Read())
                {
                    tenantsList.Add(reader[index].ToString());
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return tenantsList;
        }
    }
}