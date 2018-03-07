using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;
using System.Collections.Generic;

namespace Grid.Features.IMS.DAL
{
    public class AssetRepository : GenericRepository<Asset>, IAssetRepository
    {
        public AssetRepository(IDbContext context) : base(context)
        {

        }
        //public IList<Asset> GetAllEnrollment()
        //{
        //    var result = _entities.Assets.Include("AllocatedEmployee.User.Person").Where(i => i.DeleteOn == null).ToList();
        //    return result;
        //}
    }
}