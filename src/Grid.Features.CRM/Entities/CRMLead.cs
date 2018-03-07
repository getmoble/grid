using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.Entities
{
    public class CRMLead : UserCreatedEntityBase
    {
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CRMLeadCategory Category { get; set; }

        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Expertise { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Received On")]
        public DateTime? RecievedOn { get; set; }

        public int LeadSourceId { get; set; }
        [ForeignKey("LeadSourceId")]
        public virtual CRMLeadSource LeadSource { get; set; }
        
        public int LeadStatusId { get; set; }
        [ForeignKey("LeadStatusId")]
        public virtual CRMLeadStatus LeadStatus { get; set; }
        
        public int AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }
        
        public int? LeadSourceUserId { get; set; }//id of user if lead source is user/staff.

        [ForeignKey("LeadSourceUserId")]
        public virtual User LeadSourceUser { get; set; }
    }
}