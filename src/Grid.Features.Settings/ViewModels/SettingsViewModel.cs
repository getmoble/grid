using System.ComponentModel;
using Grid.Features.Common;

namespace Grid.Features.Settings.ViewModels
{
    public class SettingsViewModel: ViewModelBase
    {
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }

        // DropBox 
        [DisplayName("Application Key")]
        public string DropBoxApplicationKey { get; set; }

        [DisplayName("Application Secret")]
        public string DropBoxApplicationSecret { get; set; }

        // Blob
        [DisplayName("Account Name")]
        public string BlobAccountName { get; set; }

        [DisplayName("Account Key")]
        public string BlobAccountKey { get; set; }

        // Slack
        [DisplayName("Consumer Key")]
        public string SlackConsumerKey { get; set; }

        [DisplayName("Consumer Secret")]
        public string SlackConsumerSecret { get; set; }

        [DisplayName("Scopes")]
        public string SlackScopes { get; set; }

        // Email 
        public string Server { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [DisplayName("Email - From Name")]
        public string FromName { get; set; }

        [DisplayName("Email - From Email")]
        public string FromEmail { get; set; }

        [DisplayName("Administrator Email")]
        public string AdminEmail { get; set; }

        // Point of Contacts 
        [DisplayName("IT Department Level 1")]
        public int ITDepartmentLevel1 { get; set; }

        [DisplayName("IT Department Level 2")]
        public int ITDepartmentLevel2 { get; set; }

        [DisplayName("HR Department Level 1")]
        public int HRDepartmentLevel1 { get; set; }

        [DisplayName("HR Department Level 2")]
        public int HRDepartmentLevel2 { get; set; }

        [DisplayName("Sales Department Level 1")]
        public int SalesDepartmentLevel1 { get; set; }

        [DisplayName("Sales Department Level 2")]
        public int SalesDepartmentLevel2 { get; set; }

        [DisplayName("Max Timesheet Misses")]
        public int MaxTimeSheetMisses { get; set; }

        [DisplayName("Max Timesheet Approval Misses")]
        public int MaxTimeSheetApprovalMisses { get; set; }
    }
}
