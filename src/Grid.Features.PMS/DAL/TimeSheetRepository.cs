using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class TimeSheetRepository : GenericRepository<TimeSheet>, ITimeSheetRepository
    {
        public TimeSheetRepository(IDbContext context) : base(context)
        {

        }
    }
}