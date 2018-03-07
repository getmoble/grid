using Microsoft.AspNet.SignalR;

namespace Grid
{
    public class GridSignalRUserIdProvider: IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var name = request.User.Identity.Name;
            return name;
        }
    }
}