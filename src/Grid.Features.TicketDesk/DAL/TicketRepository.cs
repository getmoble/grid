using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Features.TicketDesk.DAL
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(IDbContext context) : base(context)
        {

        }
    }
}