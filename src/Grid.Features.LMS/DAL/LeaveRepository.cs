using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Features.LMS.DAL
{
    public class LeaveRepository : GenericRepository<Leave>, ILeaveRepository
    {
        public LeaveRepository(IDbContext context) : base(context)
        {

        }
    }
}