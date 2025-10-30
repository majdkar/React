using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Settings
{
    public class UnitUpdateModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? BaseUnitId { get; set; }

        public decimal? BaseUnitRatio { get; set; }

        public bool IsActive { get; set; }
    }

    public class UnitUpdateValidator : AbstractValidator<UnitUpdateModel>
    {
        public UnitUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}
