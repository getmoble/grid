using Grid.Features.PMS.Entities.Enums;
using System;

namespace Grid.Api.Models.PMS
{
    public class TaskModel : ApiModelBase
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string TaskStatusType { get; set; }
        public string PriorityType { get; set; }        
        public string Description { get; set; }
        public float? ExpectedTime { get; set; }
        public float? ActualTime { get; set; }
        public ProjectTaskStatus TaskStatus { get; set; }
        public TaskPriority Priority { get; set; }
        public int? AssigneeId { get; set; }
        public string Assignee { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public string Project { get; set; }
        public int? ParentId { get; set; }
        public string Parent { get; set; }

        public TaskModel()
        {

        }

        public TaskModel(Features.PMS.Entities.Task task)
        {
            Id = task.Id;

            if (task.Project != null)
            {               
                    Project = task.Project.Title.TrimStart();                
            }
            if (task.Parent != null)
            {
                Parent = task.Parent.Title;
            }
            if (task.Assignee != null)
            {
                if(task.Assignee.User != null)
                {
                    if (task.Assignee.User.Person != null)
                    {
                        Assignee = task.Assignee.User.Person.Name;
                    }
                }
                    
            }
            string[] words = task.Title.Split();
          
            Title = words[0] + " " + words[1];
            Code = task.Code;
            ParentId = task.ParentId;
            ProjectId = task.ProjectId;
            Description = task.Description;
            StartDate = task.StartDate;
            DueDate = task.DueDate;
            AssigneeId = task.AssigneeId;
            TaskStatusType = GetEnumDescription(task.TaskStatus);
            PriorityType = GetEnumDescription(task.Priority);
            CreatedOn = task.CreatedOn;

        }
    }
}
