using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;

namespace Grid.Features.IMS.DAL
{
    public class AssetDocumentRepository : GenericRepository<AssetDocument>, IAssetDocumentRepository
    {
        public AssetDocumentRepository(IDbContext context) : base(context)
        {

        }
    }
}