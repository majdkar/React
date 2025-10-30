using SchoolV01.Domain.Entities.ExtendedAttributes;
using SchoolV01.Domain.Entities.Misc;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Validators.Features.ExtendedAttributes.Commands
{
    public class AddEditDocumentExtendedAttributeCommandValidator : AddEditExtendedAttributeCommandValidator<int, int, Document, DocumentExtendedAttribute>
    {
        public AddEditDocumentExtendedAttributeCommandValidator(IStringLocalizer<AddEditExtendedAttributeCommandValidatorLocalization> localizer) : base(localizer)
        {
            // you can override the validation rules here
        }
    }
}