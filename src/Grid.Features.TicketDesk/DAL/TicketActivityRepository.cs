using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Features.TicketDesk.DAL
{
    public class TicketActivityRepository : GenericRepository<TicketActivity>, ITicketActivityRepository
    {
        public TicketActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}