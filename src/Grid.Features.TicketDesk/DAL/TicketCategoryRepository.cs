using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Features.TicketDesk.DAL
{
    public class TicketCategoryRepository : GenericRepository<TicketCategory>, ITicketCategoryRepository
    {
        public TicketCategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}