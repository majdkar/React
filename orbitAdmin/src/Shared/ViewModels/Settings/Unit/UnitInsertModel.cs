using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Settings
{
    public class UnitInsertModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? BaseUnitId { get; set; }

        public decimal? BaseUnitRatio { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UnitInsertValidator : AbstractValidator<UnitInsertModel>
    {
        public UnitInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}
