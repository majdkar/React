using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Requests.Mail
{
    public class RegistrationEmailModel
    {
        public string RecieverEmail { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }
    }
}
