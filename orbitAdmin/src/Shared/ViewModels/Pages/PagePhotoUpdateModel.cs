using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Pages
{
    public class PagePhotoUpdateModel
    {
        public int Id { set; get; }

        public string Image { get; set; }

        public int PageId { get; set; }
    }

    public class PagePhotoUpdateValidator : AbstractValidator<PagePhotoUpdateModel>
    {
        public PagePhotoUpdateValidator()
        {


        }
    }
}
