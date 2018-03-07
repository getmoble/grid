using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels.Project
{
    public class NewProjectViewModel: ViewModelBase
    {
        [DisplayName("Client")]
        public int ClientId { get; set; }

        [DisplayName("Title")]
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Status")]
        public ProjectStatus Status { get; set; }

        [DisplayName("Billing")]
        public Billing Billing { get; set; }

        [DisplayName("Expected Billing Amount")]
        public decimal ExpectedBillingAmount { get; set; }

        public double Rate { get; set; }
        public int? ParentId { get; set; }

        [DisplayName("Is Public")]
        public bool IsPublic { get; set; }

        [DisplayName("Inherit Members")]
        public bool InheritMembers { get; set; }
        public bool IsClosed { get; set; }

        public int[] TechnologyIds { get; set; }
    }
}