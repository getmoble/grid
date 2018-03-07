using System.ComponentModel;

namespace Grid.Providers.Email
{
    public class EmailSettingsModel
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [DisplayName("Email - From Name")]
        public string FromName { get; set; }

        [DisplayName("Email - From Email")]
        public string FromEmail { get; set; }

        [DisplayName("Administrator Email")]
        public string AdminEmail { get; set; }
    }
}