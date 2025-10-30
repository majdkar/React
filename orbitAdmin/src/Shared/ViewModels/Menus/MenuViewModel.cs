using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.Menus
{
    public class MenuViewModel
    {
        public int Id { set; get; }

        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public string Type { get; set; }

        public string Image { get; set; }

        public string File { get; set; }

        public string PageUrl { get; set; }
        public string Url { get; set; }

        public int LevelOrder { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }
        public bool IsHome { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHomeFooter { get; set; }

        public bool ShowTranslation { get; set; } = false;

        public int CategoryId { get; set; }
        public virtual MenuCategoryViewModel MenuCategory { get; set; }

        public bool IsActive { get; set; }

    }
}
