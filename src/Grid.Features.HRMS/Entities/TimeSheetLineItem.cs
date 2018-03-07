using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class TimeSheetLineItem : EntityBase
    {
        public int TimeSheetId { get; set; }
        [ForeignKey("TimeSheetId")]
        public TimeSheet TimeSheet { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int? TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        public string TaskSummary { get; set; }
        public double Effort { get; set; }
        public string Comments { get; set; }
        public int WorkType { get; set; }
    }
}