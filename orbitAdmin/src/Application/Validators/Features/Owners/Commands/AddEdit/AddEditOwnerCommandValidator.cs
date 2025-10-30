using System;
using SchoolV01.Application.Features.Owners.Commands;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Validators.Features.Owners.Commands
{
    public class AddEditOwnerCommandValidator : AbstractValidator<AddEditOwnerCommand>
    {
        public AddEditOwnerCommandValidator(IStringLocalizer<AddEditOwnerCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
			
			
RuleFor(request => request.PassportId)
                                                     .GreaterThan(0).WithMessage(x => localizer["Passport is required!"]);
			/*RuleFor(request => request.Barcode)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Barcode is required!"]);
            
            RuleFor(request => request.BrandId)
                .GreaterThan(0).WithMessage(x => localizer["Brand is required!"]);
            RuleFor(request => request.Rate)
                .GreaterThan(0).WithMessage(x => localizer["Rate must be greater than 0"]);*/
        }
    }
}