using System;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Api.Models.RMS
{
    public class RequirementUpdateModel: ApiModelBase
    {
        public int AssignedToUserId { get; set; }
        public int? ContactId { get; set; }
        public int SourceId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public BillingType? BillingType { get; set; }
        public decimal? Budget { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime? RespondedOn { get; set; }

        public int[] TechnologyIds { get; set; }

        public RequirementUpdateModel(Requirement requirement)
        {
            Id = requirement.Id;
            AssignedToUserId = requirement.AssignedToUserId;
            ContactId = requirement.ContactId;
            SourceId = requirement.SourceId;
            Title = requirement.Title;
            Description = requirement.Description;
            Url = requirement.Url;
            BillingType = requirement.BillingType;
            Budget = requirement.Budget;
            PostedOn = requirement.PostedOn;
            RespondedOn = requirement.RespondedOn;
        }
    }
}
