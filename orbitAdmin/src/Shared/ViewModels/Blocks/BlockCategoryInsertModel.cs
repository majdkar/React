using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockCategoryInsertModel
    {
        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public string BlockType { get; set; }

        public bool IsActive { get; set; } = true;

    }

    public class BlockCategoryInsertValidator : AbstractValidator<BlockCategoryInsertModel>
    {
        public BlockCategoryInsertValidator()
        {
            RuleFor(p => p.NameAr).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.BlockType).NotEmpty().WithMessage("You must enter block type");
        }
    }
}
