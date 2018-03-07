using Grid.Features.Common;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;

namespace Grid.Features.IT.DAL
{
    public class SystemSnapshotRepository : GenericRepository<SystemSnapshot>, ISystemSnapshotRepository
    {
        public SystemSnapshotRepository(IDbContext context) : base(context)
        {

        }
    }
}