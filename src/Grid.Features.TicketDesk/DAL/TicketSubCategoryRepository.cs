using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Features.TicketDesk.DAL
{
    public class TicketSubCategoryRepository : GenericRepository<TicketSubCategory>, ITicketSubCategoryRepository
    {
        public TicketSubCategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}