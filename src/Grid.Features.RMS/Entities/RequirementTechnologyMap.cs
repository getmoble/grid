using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.RMS.Entities
{
    public class RequirementTechnologyMap: EntityBase
    {
        public int RequirementId { get; set; }
        [ForeignKey("RequirementId")]
        public virtual Requirement Requirement { get; set; }

        public int TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public virtual Technology Technology { get; set; }
    }
}