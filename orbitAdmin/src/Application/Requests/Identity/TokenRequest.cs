using System.ComponentModel.DataAnnotations;

namespace SchoolV01.Application.Requests.Identity
{
    public class TokenRequest
    {
        [Required]
        public string EmailOrUserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}