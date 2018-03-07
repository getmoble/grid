using Grid.Features.Common;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Settings.Entities;

namespace Grid.Features.Settings.DAL
{
    public class GridUpdateRepository : GenericRepository<GridUpdate>, IGridUpdateRepository
    {
        public GridUpdateRepository(IDbContext context) : base(context)
        {

        }
    }
}
