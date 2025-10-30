using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Responses.Identity
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Body { get; set; }

        public string ImgPath { get; set; }

        public string FromUserId { get; set; }
        public string FromUserImageURL { get; set; }
        public string FromUserFullName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserImageURL { get; set; }
        public string ToUserFullName { get; set; }
        public bool Seen { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
