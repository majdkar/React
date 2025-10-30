using FluentValidation;

namespace SchoolV01.Shared.ViewModels.Events
{
    public class EventAttachementUpdateModel
    {
        public int Id { set; get; }

        public string File { get; set; }
        public string Name { get; set; }

        public int EventId { get; set; }
    }

    public class EventAttachementUpdateValidator : AbstractValidator<EventAttachementUpdateModel>
    {
        public EventAttachementUpdateValidator()
        {


        }
    }
}
