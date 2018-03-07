using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class ProjectActivityRepository : GenericRepository<ProjectActivity>, IProjectActivityRepository
    {
        public ProjectActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}
