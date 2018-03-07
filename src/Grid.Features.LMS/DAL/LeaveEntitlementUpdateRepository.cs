using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Features.LMS.DAL
{
    public class LeaveEntitlementUpdateRepository : GenericRepository<LeaveEntitlementUpdate>, ILeaveEntitlementUpdateRepository
    {
        public LeaveEntitlementUpdateRepository(IDbContext context) : base(context)
        {

        }
    }
}