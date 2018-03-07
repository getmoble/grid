using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.ViewModels
{
    public class ConvertLeadViewModel: ViewModelBase
    {
        public bool CreateAccount { get; set; }
        public bool CreatePotential { get; set; }

        public int LeadId { get; set; }

        [DisplayName("Do not create Potential on Conversion")]
        public bool NoPotentialRequired { get; set; }
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public int AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public virtual Features.HRMS.Entities.User AssignedToUser { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CRMPotentialCategory Category { get; set; }

        [DisplayName("Expected Amount")]
        public double? ExpectedAmount { get; set; }
        [DisplayName("Expected Close Date")]
        public DateTime? ExpectedCloseDate { get; set; }

        [DisplayName("Enquired On")]
        public DateTime? EnquiredOn { get; set; }
        public int SalesStageId { get; set; }
        [ForeignKey("SalesStageId")]
        public virtual CRMSalesStage SalesStage { get; set; }
    }
}