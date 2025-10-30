using SchoolV01.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Core.Entities
{

    public class MenuCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public bool IsActive { get; set; }
        public bool IsVisibleUser { get; set; } = false;
        public List<Menu> Menus { get; set; }
    }
}
