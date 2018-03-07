using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.Entities
{
    public class CRMLeadTechnologyMap: EntityBase
    {
        public int LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual CRMLead Lead { get; set; }

        public int TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public virtual Technology Technology { get; set; }
    }
}