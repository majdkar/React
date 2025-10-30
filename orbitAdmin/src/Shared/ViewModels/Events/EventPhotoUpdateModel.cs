using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Events
{
    public class EventPhotoUpdateModel
    {
        public int Id { set; get; }

        public string Image { get; set; }

        public int EventId { get; set; }
    }

    public class EventPhotoUpdateValidator : AbstractValidator<EventPhotoUpdateModel>
    {
        public EventPhotoUpdateValidator()
        {


        }
    }
}
