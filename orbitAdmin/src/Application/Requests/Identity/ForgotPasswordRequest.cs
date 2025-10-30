using System.ComponentModel.DataAnnotations;

namespace SchoolV01.Application.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string EmailOrUserName { get; set; }
    }
}