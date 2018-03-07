using System.Collections.Generic;
using System.Net.Mail;

namespace Grid.Providers.Email
{
    public class EmailAttachment
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }

    public class EmailContext
    {
        public bool DropEmail { get; set; }
        public string Subject { get; set; }
        public List<PlaceHolder> PlaceHolders { get; set; }
        public List<MailAddress> ToAddress { get; set; }
        public List<MailAddress> CcAddress { get; set; }
        public List<EmailAttachment> EmailAttachments { get; set; }

        public EmailContext()
        {
            PlaceHolders = new List<PlaceHolder>();
            ToAddress = new List<MailAddress>();
            CcAddress = new List<MailAddress>();
            EmailAttachments = new List<EmailAttachment>();
        }
    }
}
