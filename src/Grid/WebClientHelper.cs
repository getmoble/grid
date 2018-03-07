using System.Net;

namespace Grid
{
    public static class WebClientHelper
    {
        public static void InvokeUrl(string url)
       {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            var response = request.GetResponse();
        }
    }
}