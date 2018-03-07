using System;
using Grid.Features.Common;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Features.RMS.ViewModels
{
    public class RequirementViewModel: ViewModelBase
    {
        public string Source { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public BillingType? BillingType { get; set; }
        public decimal? Budget { get; set; }
        public RequirementStatus Status { get; set; }
        public DateTime? PostedOn { get; set; }
        public DateTime? RespondedOn { get; set; }

        public RequirementViewModel(Requirement requirement)
        {
            Id = requirement.Id;
            Source = requirement.Source.Title;
            Category = requirement.Category.Title;
            if (requirement.Title.Length > TrimLength)
                Title = requirement.Title.Substring(0, TrimLength) + "...";
            else
                Title = requirement.Title;
            BillingType = requirement.BillingType;
            Budget = requirement.Budget;
            Status = requirement.RequirementStatus;
            PostedOn = requirement.PostedOn;
            RespondedOn = requirement.RespondedOn;
            CreatedOn = requirement.CreatedOn;
        }
    }
}