using System;
using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.Events
{
    public class EventViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int RecordOrder { get; set; }

        public virtual List<EventPhotoViewModel> EventPhotos { get; set; }
        public virtual List<EventAttachementViewModel> EventAttachements { get; set; }

        public bool ShowTranslation { get; set; } = false;

        public int CategoryId { get; set; }
        public virtual EventCategoryViewModel EventCategory { get; set; }

        public string Image { get; set; }
        public string Video { get; set; }

        public string File { get; set; }
        public string Url { get; set; }

        public bool IsVisible { get; set; }

        public bool IsActive { get; set; }
    }
}
