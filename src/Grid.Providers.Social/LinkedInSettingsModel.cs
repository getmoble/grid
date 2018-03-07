using System.ComponentModel;

namespace Grid.Providers.Social
{
    public class LinkedInSettingsModel
    {
        [DisplayName("Consumer Key")]
        public string ConsumerKey { get; set; }

        [DisplayName("Consumer Secret")]
        public string ConsumerSecret { get; set; }

        [DisplayName("Scopes")]
        public string Scopes { get; set; }
    }
}