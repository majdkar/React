using System;
using SchoolV01.Application.Features.Passports.Commands;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Validators.Features.Passports.Commands
{
    public class AddEditPassportCommandValidator : AbstractValidator<AddEditPassportCommand>
    {
        public AddEditPassportCommandValidator(IStringLocalizer<AddEditPassportCommandValidator> localizer)
        {
			RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
			
            
            //RuleFor(request => request.Tax)
            //    .GreaterThan(0).WithMessage(x => localizer["Tax must be greater than 0"]);
        }
    }
}