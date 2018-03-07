using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class ChangeTaskStatusViewModel: ViewModelBase
    {
        public int TaskId { get; set; }

        [DisplayName("Task Status")]
        public ProjectTaskStatus TaskStatus { get; set; }

        public double Effort { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}