using Grid.Features.Common;
using Grid.Features.IMS.Entities;
using Grid.Features.IMS.Entities.Enums;
using PagedList;

namespace Grid.Features.IMS.ViewModels
{
    public class AssetSearchViewModel: PagedViewModelBase
    {
        public string TagNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Title { get; set; }
        public string ModelNumber { get; set; }
        public AssetState? State { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public int? VendorId { get; set; }
        public int? AllocatedUserId { get; set; }

        public IPagedList<Asset> Assets { get; set; }
    }
}