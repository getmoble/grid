using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.ViewModels
{
    public class CRMPotentialDetailsViewModel : ViewModelBase
    {
        public int? CategoryId { get; set; }
        public CRMPotentialCategory Category { get; set; }

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
        public CRMSalesStage SalesStage { get; set; }

        public int? AssignedToUserId { get; set; }

        [ForeignKey("AssignedToUserId")]
        public Features.HRMS.Entities.User AssignedToUser { get; set; }

        public int? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public CRMContact Contact { get; set; }

        public CRMPotentialDetailsViewModel(CRMPotential potential)
        {
            Id = potential.Id;
            CategoryId = potential.CategoryId;
            Category = potential.Category;
            ExpectedAmount = potential.ExpectedAmount;
            ExpectedCloseDate = potential.ExpectedCloseDate;
            Description = potential.Description;
            EnquiredOn = potential.EnquiredOn;
            SalesStageId = potential.SalesStageId;
            SalesStage = potential.SalesStage;
            AssignedToUserId = potential.AssignedToUserId;
            AssignedToUser = potential.AssignedToUser;
            ContactId = potential.ContactId;
            Contact = potential.Contact;
        }
    }
}