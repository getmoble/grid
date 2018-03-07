using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class TaskSearchViewModel : ViewModelBase
    {
        public bool HideCompleted { get; set; }
        public int? AssignedToId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }
        public ProjectTaskStatus? Status { get; set; }
        public List<TaskViewModel> Tasks { get; set; }

        public TaskSearchViewModel()
        {
            Tasks = new List<TaskViewModel>();
        }
    }
}