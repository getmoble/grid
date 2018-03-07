using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class CandidateTechnologyMapRepository : GenericRepository<CandidateTechnologyMap>, ICandidateTechnologyMapRepository
    {
        public CandidateTechnologyMapRepository(IDbContext context) : base(context)
        {

        }
    }
}