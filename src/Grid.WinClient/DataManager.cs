using Newtonsoft.Json;
using RestSharp;
using System;

namespace Grid.WinClient
{
    public static class DataManager
    {
        public static RestClient GetClient()
        {
            var client = new RestClient { BaseUrl = new Uri("http://logiticks.gridintra.net/Api/") };
            //var client = new RestClient { BaseUrl = new Uri("http://localhost:43729/Api/") };
            return client;
        }

        public static bool SendData(string data)
        {
            var client = GetClient();
            var request = new RestRequest("Sync/Create", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddParameter("application/json; charset=utf-8", data, ParameterType.RequestBody);
            var token = Properties.Settings.Default.Token;
            request.AddHeader("Auth", token);
            var response = client.Execute(request);
            return !string.IsNullOrEmpty(response.Content) && JsonConvert.DeserializeObject<bool>(response.Content);
        }
    }
}
