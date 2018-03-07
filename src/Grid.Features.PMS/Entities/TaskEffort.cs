using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.PMS.Entities
{
    public class TaskEffort: UserCreatedEntityBase
    {
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        public double Effort { get; set; }

        public string Comments { get; set; }
    }
}