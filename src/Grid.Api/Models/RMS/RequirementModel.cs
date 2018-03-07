using System;
using Grid.Features.RMS.Entities;

namespace Grid.Api.Models.RMS
{
    public class RequirementModel: ApiModelBase
    {
        public string Title { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public string BillingType { get; set; }
        public decimal? Budget { get; set; }
        public string Status { get; set; }
        public DateTime PostedOn  { get; set; }
        public DateTime? RespondedOn { get; set; }

        public RequirementModel(Requirement requirement)
        {
            Id = requirement.Id;
            Title = requirement.Title;

            if (requirement.Source != null)
            {
                Source = requirement.Source.Title;
            }

            if (requirement.Category != null)
            {
                Category = requirement.Category.Title;
            }

            BillingType = GetEnumDescription(requirement.BillingType);
            Budget = requirement.Budget;
            Status = GetEnumDescription(requirement.RequirementStatus);
            PostedOn = requirement.PostedOn;
            RespondedOn = requirement.RespondedOn;
        }
    }
}
