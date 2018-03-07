using System.ComponentModel;

namespace Grid.Providers.Slack
{
    public class SlackSettingsModel
    {
        [DisplayName("Consumer Key")]
        public string ConsumerKey { get; set; }

        [DisplayName("Consumer Secret")]
        public string ConsumerSecret { get; set; }

        [DisplayName("Scopes")]
        public string Scopes { get; set; }
    }
}