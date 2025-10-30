using FluentValidation;
using System;

namespace SchoolV01.Shared.ViewModels.Events
{
    public class EventInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int RecordOrder { get; set; }

        public int CategoryId { get; set; }

        public string Image { get; set; }
        public string Video { get; set; }

        public string File { get; set; }
        public string Url { get; set; }

        public bool IsVisible { get; set; }

        public bool IsActive { get; set; }
    }

    public class EventInsertValidator : AbstractValidator<EventInsertModel>
    {
        public EventInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.EnglishName).NotEmpty().WithMessage("You must enter English Name");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("You must choose category");
            RuleFor(p => p.RecordOrder).GreaterThanOrEqualTo(0).WithMessage("Value must be Greater or Equal to Zero");
        }
    }
}
