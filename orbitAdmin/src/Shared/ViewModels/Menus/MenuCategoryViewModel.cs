using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.Menus
{
    public class MenuCategoryViewModel
    {
        public int Id { set; get; }

        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public bool IsActive { get; set; }
        public bool IsVisableUser { get; set; } = false;
        public virtual List<MenuViewModel> Menus { get; set; }


        public bool ShowTranslation { get; set; } = false;
    }
}
