using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockCategoryUpdateModel
    {
        public int Id { set; get; }

        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public string BlockType { get; set; }

        public bool IsActive { get; set; }

    }
    public class BlockCategoryUpdateValidator : AbstractValidator<BlockCategoryUpdateModel>
    {
        public BlockCategoryUpdateValidator()
        {
            RuleFor(p => p.NameAr).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.BlockType).NotEmpty().WithMessage("You must enter block type");
        }
    }
}
