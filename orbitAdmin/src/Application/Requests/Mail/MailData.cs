using System.Collections.Generic;

namespace SchoolV01.Application.Requests.Mail
{
    public class MailData
    {
        // Receiver
        public List<MailAddress> To { get; }
        //public List<string> Bcc { get; }

        public List<MailAddress> Cc { get; }

        // Sender
        public string From { get; }

        public string DisplayName { get; }

        public string ReplyTo { get; }

        public string ReplyToName { get; }

        // Content
        public string Subject { get; }

        public string Body { get; }

        public MailData(List<MailAddress> to, string subject, string body = null, string from = null, string displayName = null, string replyTo = null, string replyToName = null, List<MailAddress> cc = null)
        {
            // Receiver
            To = to;
            //Bcc = bcc ?? new List<string>();
            Cc = cc ?? new List<MailAddress>();

            // Sender
            From = from;
            DisplayName = displayName;
            ReplyTo = replyTo;
            ReplyToName = replyToName;

            // Content
            Subject = subject;
            Body = body;
        }
    }

    public class MailAddress
    {
        public string ToName { get; set; }
        public string ToMail { get; set; }
    }
}
