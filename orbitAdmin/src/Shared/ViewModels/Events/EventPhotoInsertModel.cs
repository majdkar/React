using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Events
{
    public class EventPhotoInsertModel
    {
        public string Image { get; set; }

        public int EventId { get; set; }
    }

    public class EventPhotoInsertValidator : AbstractValidator<EventPhotoInsertModel>
    {
        public EventPhotoInsertValidator()
        {
            
        }
    }
}
