using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class CandidateDocumentRepository : GenericRepository<CandidateDocument>, ICandidateDocumentRepository
    {
        public CandidateDocumentRepository(IDbContext context) : base(context)
        {

        }
    }
}