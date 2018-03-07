using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IDbContext context) : base(context)
        {
        }
    }
}
