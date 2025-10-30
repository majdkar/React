namespace SchoolV01.Application.Requests.Mail
{
    public class OrderProductEmailModel
    {
        public string RecieverEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string FrontImageAttachmentPath { get; set; }

        public string BackImageAttachmentPath { get; set; }

    }
}
