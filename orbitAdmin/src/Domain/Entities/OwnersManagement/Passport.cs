using System;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Domain.Contracts;

namespace SchoolV01.Domain.Entities.OwnersManagement
{
    public class Passport : AuditableEntity<int>
    {
        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }
        public DateTime? ExpiryDate { get; set; }

    }
}