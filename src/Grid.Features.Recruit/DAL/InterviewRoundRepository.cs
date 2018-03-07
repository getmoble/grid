using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class InterviewRoundRepository : GenericRepository<InterviewRound>, IInterviewRoundRepository
    {
        public InterviewRoundRepository(IDbContext context) : base(context)
        {

        }
    }
}