using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Grid.AccessLogs
{
    public class AccessLog : TableEntity
    {
        public string LogDate { get; set; }
        public string LogTime { get; set; }

        public int Source { get; set; }
        public string Operation { get; set; }

        public string Data { get; set; }

        public AccessLog(int employeeId, DateTime logTime, AccessLogSource source, string operation, string data)
        {
            PartitionKey = employeeId.ToString();
            RowKey = Guid.NewGuid().ToString("N");

            LogDate = logTime.ToShortDateString();
            LogTime = logTime.ToLongTimeString();

            Source = (int)source;
            Operation = operation;

            Data = data;
        }

        public AccessLog()
        {
            
        }
    }
}
