using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Articles
{
    public class ArticleCategoryInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }

        public string CategoryType { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class ArticleCategoryInsertValidator : AbstractValidator<ArticleCategoryInsertModel>
    {
        public ArticleCategoryInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.CategoryType).NotEmpty().WithMessage("You must enter category type");
        }
    }
}
