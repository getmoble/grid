using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;

namespace Grid.Features.RMS.DAL
{
    public class RequirementCategoryRepository : GenericRepository<RequirementCategory>, IRequirementCategoryRepository
    {
        public RequirementCategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}