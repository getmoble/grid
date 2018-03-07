using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class TechnologyRepository : GenericRepository<Technology>, ITechnologyRepository
    {
        public TechnologyRepository(IDbContext context) : base(context)
        {

        }
    }
}