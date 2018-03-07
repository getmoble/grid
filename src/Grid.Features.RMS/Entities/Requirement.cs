using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Features.RMS.Entities
{
    public class Requirement : UserCreatedEntityBase
    {
        public int AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }

        public int? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual CRMContact Contact { get; set; }

        public int SourceId { get; set; }
        [ForeignKey("SourceId")]
        public virtual CRMLeadSource Source { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual RequirementCategory Category { get; set; }

        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }


        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public string Url { get; set; }

        [DisplayName("Billing Type")]
        public BillingType? BillingType { get; set; }
        public decimal? Budget { get; set; }
        
        public RequirementStatus RequirementStatus { get; set; }

        [DisplayName("Requirement Posted On")]
        public DateTime PostedOn { get; set; }

        [DisplayName("Logiticks Responsed On")]
        public DateTime? RespondedOn { get; set; }
    }
}