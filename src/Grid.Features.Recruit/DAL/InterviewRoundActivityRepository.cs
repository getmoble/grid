using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class InterviewRoundActivityRepository : GenericRepository<InterviewRoundActivity>, IInterviewRoundActivityRepository
    {
        public InterviewRoundActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}