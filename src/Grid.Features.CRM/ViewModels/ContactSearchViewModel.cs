using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.ViewModels
{
    public class ContactSearchViewModel: ViewModelBase
    {
        public int? AccountId { get; set; }
        public List<CRMContact> Contacts { get; set; }

        public ContactSearchViewModel()
        {
            Contacts = new List<CRMContact>();
        }
    }
}