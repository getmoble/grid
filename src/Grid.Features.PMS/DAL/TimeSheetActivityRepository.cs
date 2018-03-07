using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class TimeSheetActivityRepository : GenericRepository<TimeSheetActivity>, ITimeSheetActivityRepository
    {
        public TimeSheetActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}