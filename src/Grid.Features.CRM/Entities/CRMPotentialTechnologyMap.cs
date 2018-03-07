using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.Entities
{
    public class CRMPotentialTechnologyMap: EntityBase
    {
        public int PotentialId { get; set; }
        [ForeignKey("PotentialId")]
        public virtual CRMPotential Potential { get; set; }

        public int TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public virtual Technology Technology { get; set; }
    }
}
