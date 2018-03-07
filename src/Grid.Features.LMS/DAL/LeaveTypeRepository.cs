using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;

namespace Grid.Features.LMS.DAL
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(IDbContext context) : base(context)
        {

        }
    }
}