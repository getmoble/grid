namespace Grid.UI.WebApp.Hubs
{
    public interface INotificationService
    {
        void NotifyUser(string title, string message, string userCode);
    }
}