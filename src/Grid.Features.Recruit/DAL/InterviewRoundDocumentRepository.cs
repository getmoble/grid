using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class InterviewRoundDocumentRepository : GenericRepository<InterviewRoundDocument>, IInterviewRoundDocumentRepository
    {
        public InterviewRoundDocumentRepository(IDbContext context) : base(context)
        {

        }
    }
}