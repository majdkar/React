using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Countries.Commands;

namespace SchoolV01.Application.Validators.Features.ServiceTypes.Commands
{
    public class AddEditCountryCommandValidator : AbstractValidator<AddEditCountryCommand>
    {
        public AddEditCountryCommandValidator(IStringLocalizer<AddEditCountryCommand> localizer)
        {
			RuleFor(request => request.NameAr).NotEmpty().WithMessage(x => localizer["Arabic Name is required!"]);

        }
    }
}