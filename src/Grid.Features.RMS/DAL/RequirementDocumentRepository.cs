using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;

namespace Grid.Features.RMS.DAL
{
    public class RequirementDocumentRepository : GenericRepository<RequirementDocument>, IRequirementDocumentRepository
    {
        public RequirementDocumentRepository(IDbContext context) : base(context)
        {

        }
    }
}