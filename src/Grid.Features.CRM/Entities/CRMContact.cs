using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.Entities
{
    public class CRMContact : UserCreatedEntityBase
    {
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        [DataType(DataType.MultilineText)]
        public string Expertise { get; set; }
       
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        public int? ParentAccountId { get; set; }
        [ForeignKey("ParentAccountId")]
        public virtual CRMAccount ParentAccount { get; set; }
    }
}