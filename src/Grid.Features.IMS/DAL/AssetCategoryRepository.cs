using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;

namespace Grid.Features.IMS.DAL
{
    public class AssetCategoryRepository : GenericRepository<AssetCategory>, IAssetCategoryRepository
    {
        public AssetCategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}