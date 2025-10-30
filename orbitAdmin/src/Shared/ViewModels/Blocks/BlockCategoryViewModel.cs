using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockCategoryViewModel
    {
        public int Id { set; get; }

        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public string BlockType { get; set; }

        public bool IsActive { get; set; }

        public List<BlockViewModel> Blocks { get; set; }=new(new List<BlockViewModel>());


        public bool ShowTranslation { get; set; } = false;
    }
}
