using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.ViewModels
{
    public class EditTaskViewModel: CreateTaskViewModel
    {
        public EditTaskViewModel()
        {
            
        }

        public EditTaskViewModel(Task task)
        {
            Id = task.Id;
            Code = task.Code;
            Title = task.Title;
            Description = task.Description;
            ExpectedTime = task.ExpectedTime;
            ActualTime = task.ActualTime;
            TaskStatus = task.TaskStatus;
            Priority = task.Priority;
            AssigneeId = task.AssigneeId;
            StartDate = task.StartDate;
            DueDate = task.DueDate;
            ProjectId = task.ProjectId;
            CreatedOn = task.CreatedOn;
            ParentId = task.ParentId;
        }
    }
}
