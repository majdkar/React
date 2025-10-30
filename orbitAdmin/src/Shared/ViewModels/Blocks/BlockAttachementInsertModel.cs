using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockAttachementInsertModel
    {
        public string File { get; set; }

        public int BlockId { get; set; }
        public string Name { get; set; }
    }

    public class BlockAttachementInsertValidator : AbstractValidator<BlockAttachementInsertModel>
    {
        public BlockAttachementInsertValidator()
        {
            
        }
    }
}
