using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Settings
{
    public class LanguageUpdateModel
    {
        public int Id { set; get; }

        public string Name { get; set; }

        public string LanguageCode { get; set; }

        public bool IsActive { get; set; }
    }

    public class LanguageUpdateValidator : AbstractValidator<LanguageUpdateModel>
    {
        public LanguageUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.LanguageCode).NotEmpty().WithMessage("You must enter code");
        }
    }
}
