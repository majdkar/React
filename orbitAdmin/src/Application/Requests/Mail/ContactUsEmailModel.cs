using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Requests.Mail
{
    public class ContactUsEmailModel
    {
        public string SenderEmail { get; set; }

        public string FullName { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
