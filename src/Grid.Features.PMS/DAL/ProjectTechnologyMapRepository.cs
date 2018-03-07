using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class ProjectTechnologyMapRepository : GenericRepository<ProjectTechnologyMap>, IProjectTechnologyMapRepository
    {
        public ProjectTechnologyMapRepository(IDbContext context) : base(context)
        {

        }
    }
}