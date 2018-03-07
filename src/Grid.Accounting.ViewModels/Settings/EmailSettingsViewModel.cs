namespace Swift.UI.ViewModels.Settings
{
    public class EmailSettingsViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
