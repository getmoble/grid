using Newtonsoft.Json;
using RestSharp;

namespace Grid.Providers.Social
{
    public class LinkedInService
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;

        public LinkedInService(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        protected ProviderInfo GetProviderDescription()
        {
            return new ProviderInfo
            {
                BaseUrl = "https://www.linkedin.com",
                AuthorityUrl = "https://www.linkedin.com/oauth/v2/authorization",
                AccessTokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken",
                Version = "1.0"
            };
        }

        public string Authorize(string state, string callbackUrl)
        {
            var providerDescription = GetProviderDescription();
            var url = $"{providerDescription.AuthorityUrl}?response_type=code&client_id={_consumerKey}&redirect_uri={callbackUrl}&state={state}";
            return url;
        }

        public OAuthInfo ExchangeCodeForToken(string code, string state, string callbackUrl)
        {
            var providerDescription = GetProviderDescription();
            var client = new RestClient(providerDescription.BaseUrl);

            var request = new RestRequest("oauth/v2/accessToken", Method.POST);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", callbackUrl);
            request.AddParameter("client_id", _consumerKey);
            request.AddParameter("client_secret", _consumerSecret);

            var rawResponse = client.Execute(request);

            var response = JsonConvert.DeserializeObject<OAuthInfo>(rawResponse.Content);
            return response;
        }
    }
}
