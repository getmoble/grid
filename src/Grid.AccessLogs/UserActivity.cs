using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Grid.AccessLogs
{
    public class UserActivity : TableEntity
    {
        public string Name { get; set; }
        public string ProcessName { get; set; }
        public string WindowTitle { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActivity { get; set; }
        public string AssetId { get; set; }
    }
}
