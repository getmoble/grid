using Newtonsoft.Json;
using RestSharp;

namespace Grid.Providers.Slack
{
    public class SlackService
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _scopes;

        public SlackService(string consumerKey, string consumerSecret, string scopes)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _scopes = scopes;
        }

        public SlackService(SlackSettingsModel slackSettings)
        {
            _consumerKey = slackSettings.ConsumerKey;
            _consumerSecret = slackSettings.ConsumerSecret;
            _scopes = slackSettings.Scopes;
        }

        protected ProviderInfo GetProviderDescription()
        {
            return new ProviderInfo
            {
                BaseUrl = "https://slack.com/api",
                AuthorityUrl = "https://slack.com/oauth/authorize",
                AccessTokenEndpoint = "https://slack.com/api/oauth.access",
                Version = "1.0"
            };
        }

        public string Authorize(string callbackUrl)
        {
            var providerDescription = GetProviderDescription();
            var url = $"{providerDescription.AuthorityUrl}?client_id={_consumerKey}&scope={_scopes}&redirect_uri={callbackUrl}";
            return url;
        }

        public OAuthInfo ExchangeCodeForToken(string code)
        {
            var providerDescription = GetProviderDescription();
            var url = $"oauth.access?client_id={_consumerKey}&client_secret={_consumerSecret}&code={code}";
            var client = new RestClient(providerDescription.BaseUrl);
            var request = new RestRequest(url, Method.GET);
            var rawResponse = client.Execute(request);

            var response = JsonConvert.DeserializeObject<OAuthInfo>(rawResponse.Content);
            return response;
        }

        public void SendMessageToChannel(string channelName, string message, string token)
        {
            var providerDescription = GetProviderDescription();
            var url = $"chat.postMessage?token={token}&channel={channelName}&text={message}";
            var client = new RestClient(providerDescription.BaseUrl);
            var request = new RestRequest(url, Method.GET);
            var rawResponse = client.Execute(request);
        }
    }
}
