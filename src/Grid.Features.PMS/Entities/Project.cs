using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.Entities
{
    [DataContract]
    public class Project : UserCreatedEntityBase
    {
        [DisplayName("Client")]
        public int ClientId { get; set; }
        
        [ForeignKey("ClientId")]
        public CRMContact Client { get; set; }

        [DisplayName("Title")]
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Status")]
        public ProjectStatus? Status { get; set; }

        [DisplayName("Project Type")]
        public ProjectType? ProjectType { get; set; }

        [DisplayName("Billing")]
        public Billing Billing { get; set; }

        [DisplayName("Expected Billing Amount")]
        public decimal ExpectedBillingAmount { get; set; }

        public double Rate { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [DisplayName("Sub Project Of")]
        public Project ParentProject { get; set; }

        [DisplayName("Is Public")]
        public bool IsPublic { get; set; }
        
        [DisplayName("Inherit Members")]
        public bool InheritMembers { get; set; }
        public bool IsClosed { get; set; }

        // This stores the serialized settings for the Project.
        public string Setting { get; set; }
    }
}
