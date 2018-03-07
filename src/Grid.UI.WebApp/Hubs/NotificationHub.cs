using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Grid.UI.WebApp.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        // Use this variable to track user count
        private static int _userCount;
        public override Task OnConnected()
        {
            _userCount ++;
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            _userCount++;
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _userCount --;
            return base.OnDisconnected(stopCalled);
        }

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}