using Grid.Features.HRMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.PMS.Entities
{
    public class ProjectBillingCorrection : UserCreatedEntityBase
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public string Comments { get; set; }
        public decimal BillingHours { get; set; }
    }
}
