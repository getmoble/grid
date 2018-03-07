using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class CreateTaskViewModel: ViewModelBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DisplayName("Expected Time")]
        [Required]
        public float? ExpectedTime { get; set; }

        [DisplayName("Actual Time")]
        public float? ActualTime { get; set; }

        [DisplayName("Task Status")]
        [Required]
        public ProjectTaskStatus TaskStatus { get; set; }

        [DisplayName("Task Priority")]
        public TaskPriority Priority { get; set; }

        public int? AssigneeId { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Due Date")]
        [UIHint("Date")]
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }

        public int? ParentId { get; set; }
    }
}
