using Grid.Providers.Blob;
using Grid.Providers.Email;
using Grid.Providers.Slack;
using System.ComponentModel;

namespace Grid.Features.Settings.Models
{
    public enum StorageType
    {
        AzureBlob,
        DropBox
    }

    public class SettingsModel
    {
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }

        public TimeSheetSettings TimeSheetSettings { get; set; }

        public StorageType StorageType { get; set; }

        public BlobSettingsModel BlobSettings { get; set; }

        public DropBoxSettings DropBoxSettings { get; set; }

        public SlackSettingsModel SlackSettings { get; set; }

        public EmailSettingsModel EmailSettings { get; set; }

        public PointOfContactSettingsModel POCSettings { get; set; }

        public SettingsModel()
        {
            TimeSheetSettings = new TimeSheetSettings();
            EmailSettings = new EmailSettingsModel();
            BlobSettings = new BlobSettingsModel();
            DropBoxSettings = new DropBoxSettings();
            SlackSettings = new SlackSettingsModel();
            POCSettings = new PointOfContactSettingsModel();
        }
    }
}
