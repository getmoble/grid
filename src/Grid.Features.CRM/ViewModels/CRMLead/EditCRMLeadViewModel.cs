namespace Grid.Features.CRM.ViewModels.CRMLead
{
    public class EditCRMLeadViewModel: NewCRMLeadViewModel
    {
        public EditCRMLeadViewModel()
        {
            
        }

        public EditCRMLeadViewModel(Features.CRM.Entities.CRMLead crmLead)
        {
            Id = crmLead.Id;
            CategoryId = crmLead.CategoryId;
            PersonId = crmLead.PersonId;
            Person = crmLead.Person;
            Expertise = crmLead.Expertise;
            Description = crmLead.Description;
            RecievedOn = crmLead.RecievedOn;
            LeadSourceId = crmLead.LeadSourceId;
            LeadStatusId = crmLead.LeadStatusId;
            AssignedToUserId = crmLead.AssignedToUserId;
            CreatedOn = crmLead.CreatedOn;
        }
    }
}