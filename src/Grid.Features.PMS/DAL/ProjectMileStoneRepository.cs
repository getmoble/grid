using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class ProjectMileStoneRepository : GenericRepository<ProjectMileStone>, IProjectMileStoneRepository
    {
        public ProjectMileStoneRepository(IDbContext context) : base(context)
        {

        }
    }
}