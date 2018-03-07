using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Features.RMS.ViewModels.Requirements
{
    public class NewRequirementViewModel: ViewModelBase
    {
        public int AssignedToUserId { get; set; }
        public int? ContactId { get; set; }
        public int SourceId { get; set; }
        public int CategoryId { get; set; }
        public int[] TechnologyIds { get; set; }

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

        [DisplayName("Requirement Posted On")]
        public DateTime PostedOn { get; set; }
    }
}