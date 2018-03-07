using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;

namespace Grid.Features.IMS.DAL
{
    public class AssetAllocationRepository : GenericRepository<AssetAllocation>, IAssetAllocationRepository
    {
        public AssetAllocationRepository(IDbContext context) : base(context)
        {

        }
    }
}