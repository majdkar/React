using System;
using SchoolV01.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Domain.Entities.OwnersManagement
{
    public class Owner : AuditableEntity<int>
    {
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }

        public string Description { get; set; }
		
public int PassportId { get; set; }
public Passport Passport { get; set; }
       // public decimal Rate { get; set; }
        //public int BrandId { get; set; }
        //public Brand Brand { get; set; }
    }
}