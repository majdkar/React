using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Events
{
    public class EventCategoryInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }


        public string Image { get; set; }

        public bool IsActive { get; set; }

        public int RecordOrder { get; set; }
    }

    public class EventCategoryInsertValidator : AbstractValidator<EventCategoryInsertModel>
    {
        public EventCategoryInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.EnglishName).NotEmpty().WithMessage("You must enter English Name");

            RuleFor(p => p.RecordOrder).GreaterThanOrEqualTo(0).WithMessage("Value must be Greater or Equal to Zero");
        }
    }
}
