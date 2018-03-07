using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class RoleMemberRepository : GenericRepository<RoleMember>, IRoleMemberRepository
    {
        public RoleMemberRepository(IDbContext context) : base(context)
        {

        }
    }
}
