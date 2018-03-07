using System;
using Grid.AccessLogs;

namespace AccessLogTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var service = new CloudTableService();
            
            // Display Logs
            var logs = service.GetAllLogsForUser(2);
            foreach (var accessLog in logs)
            {
                Console.WriteLine(accessLog.LogTime);
            }

            // Create new Entity
            var newLog = new AccessLog(2, DateTime.UtcNow, AccessLogSource.GridDesktop, "HeartBeat", "");
            service.AddLog(newLog);

            // Display Logs
            logs = service.GetAllLogsForUser(2);
            foreach (var accessLog in logs)
            {
                Console.WriteLine(accessLog.LogTime);
            }
        }
    }
}
