
using Grid.Features.Common;
using Grid.Providers.Email;

namespace Grid.Features.TicketDesk.Services.Interfaces
{
    public interface ITicketService
    {
        OperationResult<bool> Delete(int id);
        int? GetPointOfContact(string department);

        EmailContext ComposeEmailContextForTicketCreated(int ticketId);
        EmailContext ComposeEmailContextForTicketUpdated(int ticketId);
        EmailContext ComposeEmailContextForTicketMissed(int ticketId);
    }
}
