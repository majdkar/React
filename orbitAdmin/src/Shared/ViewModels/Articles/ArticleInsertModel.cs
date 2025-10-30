using FluentValidation;
using System;

namespace SchoolV01.Shared.ViewModels.Articles
{
    public class ArticleInsertModel
    {
        public string Title { set; get; }

        public DateTime CreateDate { get; set; }

        public bool IsArchived { get; set; }

        public bool IsActive { get; set; } = true;

        public string Description { set; get; }

        public string Language { get; set; }

        public int CategoryId { get; set; }
        public virtual ArticleCategoryViewModel ArticleCategory { get; set; }

        public string Image { get; set; }

        public string File { get; set; }

        public int RecordOrder { get; set; }

        public string Slug { get; set; }
    }

    public class ArticleInsertValidator : AbstractValidator<ArticleInsertModel>
    {
        public ArticleInsertValidator()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage("You must enter title");
            RuleFor(p => p.Description).NotEmpty().WithMessage("You must enter  description");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("You must choose category");
            RuleFor(p => p.RecordOrder).GreaterThanOrEqualTo(0).WithMessage("Value must be Greater or Equal to Zero");
        }
    }
}
