using Grid.Features.Common;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;

namespace Grid.Features.IT.DAL
{
    public class SoftwareCategoryRepository : GenericRepository<SoftwareCategory>, ISoftwareCategoryRepository
    {
        public SoftwareCategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}