using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;

namespace Grid.Features.RMS.DAL
{
    public class RequirementTechnologyMapRepository : GenericRepository<RequirementTechnologyMap>, IRequirementTechnologyMapRepository
    {
        public RequirementTechnologyMapRepository(IDbContext context) : base(context)
        {

        }
    }
}