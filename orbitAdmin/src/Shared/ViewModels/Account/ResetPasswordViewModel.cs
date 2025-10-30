using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string CurrentPassword { get; set; }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(p => p.NewPassword).NotEmpty().WithMessage("You must enter New Password");
            RuleFor(p => p.ConfirmPassword).NotEmpty().Matches(p => p.NewPassword).WithMessage("The password and confirmation password do not match");
            RuleFor(p => p.CurrentPassword).NotEmpty().WithMessage("You must enter your current password");
        }
    }
}
