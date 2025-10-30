using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Domain.Interfaces.Chat
{
    public interface IChatUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Column(TypeName = "text")]
        public string PictureUrl { get; set; }
    }
}