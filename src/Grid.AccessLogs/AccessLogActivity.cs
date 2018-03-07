using Newtonsoft.Json;

namespace Grid.AccessLogs
{
    public class AccessLogActivity
    {
        public string Type { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
