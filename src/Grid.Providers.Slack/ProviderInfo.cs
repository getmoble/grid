namespace Grid.Providers.Slack
{
    public class ProviderInfo
    {
        public string BaseUrl { get; set; }
        public string AuthorityUrl { get; set; }
        public string RequestTokenEndpoint { get; set; }
        public string UserAuthorizationEndpoint { get; set; }
        public string AccessTokenEndpoint { get; set; }
        public string Version { get; set; }
    }
}
