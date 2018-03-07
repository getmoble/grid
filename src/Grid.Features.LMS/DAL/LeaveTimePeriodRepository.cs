using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Features.LMS.DAL
{
    public class LeaveTimePeriodRepository : GenericRepository<LeaveTimePeriod>, ILeaveTimePeriodRepository
    {
        public LeaveTimePeriodRepository(IDbContext context) : base(context)
        {

        }
    }
}