using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.Events
{
    public class EventCategoryViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public string Image { get; set; }

        public bool IsActive { get; set; }

        public int RecordOrder { get; set; }

        public virtual List<EventViewModel> Events { get; set; }


        public bool ShowTranslation { get; set; } = false;
    }
}
