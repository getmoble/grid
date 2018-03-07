using Grid.Features.Common;
using Grid.Features.IMS.Entities;
using PagedList;

namespace Grid.Features.IMS.ViewModels
{
    public class VendorSearchViewModel: PagedViewModelBase
    {
        public IPagedList<Vendor> Vendors { get; set; }
    }
}