using Grid.Infrastructure.Extensions;
using Microsoft.AspNet.SignalR;

namespace Grid.Hubs
{
    public class NotificationService: INotificationService
    {
        private readonly IHubContext _context;

        public NotificationService()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }

        public void NotifyUser(string title, string message, string userCode)
        {
            if (_context != null)
            {
                _context.Clients.User(userCode).addNewMessageToPage(title, message.Truncate(80));
            }
        }
    }
}