using System;
using System.Net.Mail;

namespace Grid.Providers.Email
{
    public class EmailService
    {
        private readonly EmailSettingsModel _emailSettings;

        public EmailService(EmailSettingsModel emailSettings)
        {
            _emailSettings = emailSettings;
        }

        private SmtpClient GetClient(EmailSettingsModel emailSettings)
        {
            return new SmtpClient
            {
                Port = emailSettings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(emailSettings.Username, emailSettings.Password),
                Host = emailSettings.Server
            };
        }

        public bool SendEmail(MailMessage message)
        {
#if !DEBUG
            try
            {
                var client = GetClient(_emailSettings);
                client.Send(message);
              Console.WriteLine("Successfully send");
                return true;
            }
            catch(Exception ex)
            {
               Console.WriteLine("Exception Message:" + ex.Message);
                return false;
              
            }
          
#endif
            return true;
        }
    }
}
