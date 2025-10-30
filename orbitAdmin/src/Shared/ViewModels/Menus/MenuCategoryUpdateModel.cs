using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Menus
{
    public class MenuCategoryUpdateModel
    {
        public int Id { set; get; }

        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }
        public bool IsVisibleUser { get; set; } = false;
        public bool IsActive { get; set; }
    }

    public class MenuCategoryUpdateValidator : AbstractValidator<MenuCategoryUpdateModel>
    {
        public MenuCategoryUpdateValidator()
        {
            RuleFor(p => p.NameAr).NotEmpty().WithMessage("You must enter Name");

        }
    }
}
