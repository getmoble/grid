using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Features.LMS.DAL
{
    public class LeaveEntitlementRepository : GenericRepository<LeaveEntitlement>, ILeaveEntitlementRepository
    {
        public LeaveEntitlementRepository(IDbContext context) : base(context)
        {

        }
    }
}