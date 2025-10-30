using SchoolV01.Shared.ViewModels.Pages;
using System;
using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockViewModel
    {
        public int Id { set; get; }

        public string NameAr { set; get; }

        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public string DescriptionAr1 { set; get; }
        public string DescriptionEn1 { set; get; }
        public string DescriptionGe1 { set; get; }

        public string DescriptionAr2 { set; get; }
        public string DescriptionEn2 { set; get; }
        public string DescriptionGe2 { set; get; }

        public string DescriptionAr3 { set; get; }
        public string DescriptionEn3 { set; get; }
        public string DescriptionGe3 { set; get; }
        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }
        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }

        public DateTime? Date { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }

        public string Location { set; get; }
        public int CategoryId { get; set; }
        public virtual BlockCategoryViewModel BlockCategory { get; set; }

        public bool IsVisible { get; set; }

        public  string Image { get; set; }

        public string Url { get; set; }
        public string Url1 { get; set; }

        public string File { get; set; }

        public bool IsActive { get; set; }
        public int AuthorId { get; set; }
        public DateTime? CreateAt { set; get; }
        public int RecordOrder { get; set; }
        public int? ParentId { get; set; }
        public string Keywords { get; set; }
        public string SeoDescription { get; set; }
        public virtual List<BlockPhotoViewModel> BlockPhotos { get; set; }
        public virtual List<BlockAttachementViewModel> BlockAttachements { get; set; }
        public virtual BlockSeoViewModel BlockSeo { get; set; }

        public bool ShowTranslation { get; set; } = false;
    }
}
