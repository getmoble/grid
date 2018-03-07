using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.Entities
{
    public class Task : UserCreatedEntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Expected Time")]
        public float? ExpectedTime { get; set; }

        [DisplayName("Actual Time")]
        public float? ActualTime { get; set; }

        [DisplayName("Task Status")]
        public ProjectTaskStatus TaskStatus { get; set; }
        public TaskBilling TaskBilling { get; set; }


        [DisplayName("Task Priority")]
        public TaskPriority Priority { get; set; }
        public int? AssigneeId { get; set; }
        [ForeignKey("AssigneeId")]
        [DisplayName("Assignee")]
        public Employee Assignee { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Due Date")]
        [UIHint("Date")]
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [DisplayName("Project")]
        public Project Project { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [DisplayName("Parent Task")]
        public Task Parent { get; set; }
    }
}
