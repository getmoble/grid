using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class CandidateActivityRepository : GenericRepository<CandidateActivity>, ICandidateActivityRepository
    {
        public CandidateActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}