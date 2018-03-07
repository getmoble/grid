using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.Entities
{
    public class CRMPotential : UserCreatedEntityBase
    {
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CRMPotentialCategory Category { get; set; }

        [DisplayName("Expected Amount")]
        public double? ExpectedAmount { get; set; }
        [DisplayName("Expected Close Date")]
        public DateTime? ExpectedCloseDate { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Enquired On")]
        public DateTime? EnquiredOn { get; set; }
        public int SalesStageId { get; set; }
        [ForeignKey("SalesStageId")]
        public virtual CRMSalesStage SalesStage { get; set; }
        
        public int? AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }
        
        public int? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual CRMContact Contact { get; set; }
    }
}