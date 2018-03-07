using System.ComponentModel;

namespace Grid.Providers.Blob
{
    public class BlobSettingsModel
    {
        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Account Key")]
        public string AccountKey { get; set; }
    }
}