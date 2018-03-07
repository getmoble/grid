using System.ComponentModel;

namespace Grid.Providers.Blob
{
    public class DropBoxSettings
    {
        [DisplayName("Application Key")]
        public string ApplicationKey { get; set; }

        [DisplayName("Application Secret")]
        public string ApplicationSecret { get; set; }

        public string AccessToken { get; set; }
    }
}
