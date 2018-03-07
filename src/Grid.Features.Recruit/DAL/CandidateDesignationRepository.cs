using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class CandidateDesignationRepository : GenericRepository<CandidateDesignation>, ICandidateDesignationRepository
    {
        public CandidateDesignationRepository(IDbContext context) : base(context)
        {

        }
    }
}