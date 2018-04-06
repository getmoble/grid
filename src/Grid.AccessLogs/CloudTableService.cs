using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Grid.AccessLogs
{
    public class CloudTableService
    {
        private static readonly string account = "";
        private static readonly string key = "";

      
        public static CloudStorageAccount GetConnectionString()
        {
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={account};AccountKey={key}";
            return CloudStorageAccount.Parse(connectionString);
        }

        public void AddLog(AccessLog log)
        {
            var storageAccount = GetConnectionString();
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("accesslog");
            table.CreateIfNotExists();
            var insertOperation = TableOperation.Insert(log);
            table.Execute(insertOperation);
        }

        public void AddBatch(TableBatchOperation batchOperation)
        {
            var storageAccount = GetConnectionString();
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("userActivity");
            table.CreateIfNotExists();
            table.ExecuteBatch(batchOperation);
        }

        public List<AccessLog> GetAllLogsForUser(int userId)
        {
            var storageAccount = GetConnectionString();
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("accesslog");
            table.CreateIfNotExists();
            var query = new TableQuery<AccessLog>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId.ToString()));
            var results = table.ExecuteQuery(query).ToList();
            return results;
        }
    }
}
