using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class ProjectMemberRoleRepository : GenericRepository<ProjectMemberRole>, IProjectMemberRoleRepository
    {
        public ProjectMemberRoleRepository(IDbContext context) : base(context)
        {

        }
    }
}
