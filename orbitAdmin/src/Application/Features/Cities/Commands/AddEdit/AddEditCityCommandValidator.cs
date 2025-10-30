using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Cities.Commands;

namespace SchoolV01.Application.Validators.Features.ServiceTypes.Commands
{
    public class AddEditCityCommandValidator : AbstractValidator<AddEditCityCommand>
    {
        public AddEditCityCommandValidator(IStringLocalizer<AddEditCityCommand> localizer)
        {
			RuleFor(request => request.NameAr).NotEmpty().WithMessage(x => localizer["Arabic Name is required!"]);
            RuleFor(request => request.CountryId).GreaterThan(0).WithMessage(x => localizer["Country is required!"]);

        }
    }
}