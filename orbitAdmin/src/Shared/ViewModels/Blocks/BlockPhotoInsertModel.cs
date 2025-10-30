using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockPhotoInsertModel
    {
        public string Image { get; set; }

        public int BlockId { get; set; }
    }

    public class BlockPhotoInsertValidator : AbstractValidator<BlockPhotoInsertModel>
    {
        public BlockPhotoInsertValidator()
        {
            
        }
    }
}
