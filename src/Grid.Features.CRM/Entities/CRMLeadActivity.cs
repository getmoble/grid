using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.CRM.Entities
{
    public class CRMLeadActivity: UserCreatedEntityBase
    {
        public int CRMLeadId { get; set; }
        [ForeignKey("CRMLeadId")]
        public virtual CRMLead CRMLead { get; set; }

        public string Title { get; set; }

        [DisplayName("New Activity")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}