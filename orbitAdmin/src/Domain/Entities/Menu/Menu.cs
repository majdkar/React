using SchoolV01.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using DentalMgmt.DataContext.Resources;

namespace SchoolV01.Core.Entities
{

    public class Menu 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
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

        [InverseProperty("Children")]
        public int? ParentId { get; set; }
        public Menu Parent { get; set; }

        public ICollection<Menu> Children { get; set; }

        public bool IsHome { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHomeFooter { get; set; }

        [ForeignKey("MenuCategory")]
        public int CategoryId { get; set; }
        public MenuCategory MenuCategory { get; set; }

        public bool IsActive { get; set; }

    }
}
