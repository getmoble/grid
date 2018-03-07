using Grid.Features.Common;
using PagedList;

namespace Grid.Features.CRM.ViewModels.CRMLead
{
    public class CRMLeadSearchViewModel : PagedViewModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string City { get; set; }

        public int? AssignedToUserId { get; set; }
        public int? LeadSourceId { get; set; }
        public int? CategoryId { get; set; }
        public int? LeadStatusId { get; set; }

        public IPagedList<Entities.CRMLead> Leads { get; set; }
    }
}