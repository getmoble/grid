namespace Grid.Features.Common
{
    public class EventData
    {
        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool allDay { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}