namespace Grid.Areas.PMS.Models
{
    public class TimeSheetLineItemModel
    {
        public int ProjectId { get; set; }
        public int? TaskId { get; set; }
        public string TaskSummary { get; set; }
        public double Effort { get; set; }
        public string Comments { get; set; }
        public int WorkType { get; set; }
    }
}