using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class TaskDetailsViewModel: ViewModelBase
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

        [DisplayName("Task Priority")]
        public TaskPriority Priority { get; set; }
        public TaskBilling TaskBilling { get; set; }

        public int? AssigneeId { get; set; }
        public Features.HRMS.Entities.Employee Assignee { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Due Date")]
        [UIHint("Date")]
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
        [DisplayName("Project")]
        public Entities.Project Project { get; set; }

        public int? ParentId { get; set; }
        [DisplayName("Parent Task")]
        public Task Parent { get; set; }


        public Features.HRMS.Entities.User CreatedByUser { get; set; }
        public int CreatedByUserId { get; set; }

        public List<TaskActivity> TaskActivities { get; set; }

        public TaskDetailsViewModel()
        {
            TaskActivities = new List<TaskActivity>();
        }
    }
}
