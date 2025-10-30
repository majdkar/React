using System.ComponentModel.DataAnnotations;

namespace SchoolV01.Application.Requests.Identity
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string EmailOrUserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
    public class ResetPasswordAndEmailRequest
    {
        [EmailAddress]
        public string OldEmail { get; set; }
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }


        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
    public class ResetPasswordUserRequest
    {
        [Required]
        [EmailAddress]
        public string EmailOrUserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}