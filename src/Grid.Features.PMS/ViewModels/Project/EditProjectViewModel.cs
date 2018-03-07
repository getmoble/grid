namespace Grid.Features.PMS.ViewModels.Project
{
    public class EditProjectViewModel: NewProjectViewModel
    {
        public EditProjectViewModel()
        {
            
        }

        public EditProjectViewModel(Entities.Project project)
        {
            Id = project.Id;
            ClientId = project.ClientId;
            Title = project.Title;
            Description = project.Description;
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            Status = project.Status.Value;
            Billing = project.Billing;
            ExpectedBillingAmount = project.ExpectedBillingAmount;
            Rate = project.Rate;
            ParentId = project.ParentId;
            IsPublic = project.IsPublic;
            InheritMembers = project.InheritMembers;
            IsClosed = project.IsClosed;
            CreatedOn = project.CreatedOn;
        }
    }
}