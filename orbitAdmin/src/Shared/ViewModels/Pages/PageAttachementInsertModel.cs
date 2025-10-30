using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Pages
{
    public class PageAttachementInsertModel
    {
        public string File { get; set; }

        public int PageId { get; set; }
        public string Name { get; set; }
    }

    public class PageAttachementInsertValidator : AbstractValidator<PageAttachementInsertModel>
    {
        public PageAttachementInsertValidator()
        {
            
        }
    }
}
