using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Pages
{
    public class PagePhotoInsertModel
    {
        public string Image { get; set; }

        public int PageId { get; set; }
    }

    public class PagePhotoInsertValidator : AbstractValidator<PagePhotoInsertModel>
    {
        public PagePhotoInsertValidator()
        {
            
        }
    }
}
