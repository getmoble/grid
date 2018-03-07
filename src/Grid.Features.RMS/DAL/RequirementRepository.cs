using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;

namespace Grid.Features.RMS.DAL
{
    public class RequirementRepository : GenericRepository<Requirement>, IRequirementRepository
    {
        public RequirementRepository(IDbContext context) : base(context)
        {

        }
    }
}