using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.Pages
{
    public class PageViewModel
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

        public string Image { get; set; }

        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }

        public string GeoLink { get; set; }

        public string Type { get; set; }

        public bool IsActive { get; set; }

        public int RecordOrder { get; set; }
        public int MenuId { get; set; }
        public string Url { get; set; }
        public virtual List<PagePhotoViewModel> PagePhotos { get; set; }
      
        public virtual PageSeoViewModel PageSeo { get; set; }

        public virtual List<PageAttachementViewModel> PageAttachements { get; set; }
       
    }
}
