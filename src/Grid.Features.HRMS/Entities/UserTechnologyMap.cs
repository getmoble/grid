using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class UserTechnologyMap: EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public virtual Technology Technology { get; set; }
    }
}