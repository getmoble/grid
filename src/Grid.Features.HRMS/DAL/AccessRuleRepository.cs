using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class AccessRuleRepository : GenericRepository<AccessRule>, IAccessRuleRepository
    {
        public AccessRuleRepository(IDbContext context) : base(context)
        {

        }
    }
}