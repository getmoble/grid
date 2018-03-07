using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class TimeSheetLineItemRepository : GenericRepository<TimeSheetLineItem>, ITimeSheetLineItemRepository
    {
        public TimeSheetLineItemRepository(IDbContext context) : base(context)
        {

        }
    }
}