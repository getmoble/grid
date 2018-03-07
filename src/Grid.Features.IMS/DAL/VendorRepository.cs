using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;

namespace Grid.Features.IMS.DAL
{
    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(IDbContext context) : base(context)
        {

        }
    }
}