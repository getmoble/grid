using Grid.Features.Attendance.DAL.Interfaces;
using Grid.Features.Common;

namespace Grid.Features.Attendance.DAL
{
    public class AttendanceRepository : GenericRepository<Entities.Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(IDbContext context) : base(context)
        {

        }
    }
}