using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class MissedTimeSheetRepository : GenericRepository<MissedTimeSheet>, IMissedTimeSheetRepository
    {
        public MissedTimeSheetRepository(IDbContext context) : base(context)
        {

        }
    }
}
