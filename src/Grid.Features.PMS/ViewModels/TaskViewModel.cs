using System;
using Grid.Features.Common;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class TaskViewModel: ViewModelBase
    {
        public string AssignedTo { get; set; }
        public int ProjectId { get; set; }
        public string Project { get; set; }
        public string Title { get; set; }
        public float? ExpectedTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }  

        public TaskViewModel(Task task)
        {
            Id = task.Id;
            Code = task.Code;

            AssignedTo = task.Assignee.User.Person.Name;
            ProjectId = task.ProjectId;
            Project = task.Project.Title;
            
            if (task.Title.Length > TrimLength)
                Title = task.Title.Substring(0, TrimLength) + "...";
            else
                Title = task.Title;

            StartDate = task.StartDate;
            ExpectedTime = task.ExpectedTime;
            DueDate = task.DueDate;
            Status = task.TaskStatus;
            Priority = task.Priority;
            CreatedOn = task.CreatedOn;
        }
    }
}