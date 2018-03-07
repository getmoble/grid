using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.CRM.Entities
{
    public class CRMPotentialActivity: UserCreatedEntityBase
    {
        public int CRMPotentialId { get; set; }
        [ForeignKey("CRMPotentialId")]
        public virtual CRMPotential CRMPotential { get; set; }

        public string Title { get; set; }

        [DisplayName("New Activity")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}