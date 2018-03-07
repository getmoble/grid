using System.Web;

namespace Grid.Infrastructure
{
    public static class HttpCacheWrapper
    {
        public static T GetFromSession<T>(string key)
        {
            return (T)HttpRuntime.Cache[key];
        }

        public static void SetInSession(string key, object value)
        {
            HttpRuntime.Cache[key] = value;
        }
    }
}
