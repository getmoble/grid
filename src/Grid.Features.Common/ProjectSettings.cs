namespace Grid.Features.Common
{
    public class ProjectSettings
    {
        public int ProjectId { get; set; }
        public bool NeedDocuments { get; set; }
        public bool NeedTasks { get; set; }
        public bool NeedMileStones { get; set; }
        public bool NeedCalendar { get; set; }
        public bool NeedBilling { get; set; }
    }
}