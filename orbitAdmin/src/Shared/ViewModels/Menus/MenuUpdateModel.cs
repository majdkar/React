using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Menus
{
    public class MenuUpdateModel
    {
        public int Id { set; get; }

        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public string Type { get; set; }

        public string Image { get; set; }

        public string File { get; set; }

        public string PageUrl { get; set; }
        public string Url { get; set; }

        public int LevelOrder { get; set; }
        public bool IsHome { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHomeFooter { get; set; }
        public int? ParentId { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }
    }

    public class MenuUpdateValidator : AbstractValidator<MenuUpdateModel>
    {
        public MenuUpdateValidator()
        {
            RuleFor(p => p.NameAr).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("You must choose Category");
            RuleFor(p => p.LevelOrder).GreaterThanOrEqualTo(0).WithMessage("Value must be Greater or Equal to Zero");
        }
    }
}
