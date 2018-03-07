using System.Web;

namespace Grid.Infrastructure
{
    public static class HttpSessionWrapper
    {
        public static T GetFromSession<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        public static void SetInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static UserInfo GetUserInfo(string key)
        {
            return GetFromSession<UserInfo>(key);
        }

        public static void SetUserInfo(string key, UserInfo value)
        {
            SetInSession(key, value);
        }
    }
}
