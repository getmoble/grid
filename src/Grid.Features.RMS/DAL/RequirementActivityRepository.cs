using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;

namespace Grid.Features.RMS.DAL
{
    public class RequirementActivityRepository : GenericRepository<RequirementActivity>, IRequirementActivityRepository
    {
        public RequirementActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}