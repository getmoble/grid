using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Features.RMS.ViewModels.Requirements
{
    public class EditRequirementViewModel: NewRequirementViewModel
    {
        public RequirementStatus RequirementStatus { get; set; }
        public EditRequirementViewModel()
        {
            
        }

        public EditRequirementViewModel(Requirement requirement)
        {
            Id = requirement.Id;
            AssignedToUserId = requirement.AssignedToUserId;
            ContactId = requirement.ContactId;
            SourceId = requirement.SourceId;
            CategoryId = requirement.CategoryId;
            Title = requirement.Title;
            Description = requirement.Description;
            Url = requirement.Url;
            BillingType = requirement.BillingType;
            Budget = requirement.Budget;
            RequirementStatus = requirement.RequirementStatus;
            PostedOn = requirement.PostedOn;
            CreatedOn = requirement.CreatedOn;
        }
    }
}