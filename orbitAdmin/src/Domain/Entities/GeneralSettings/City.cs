using SchoolV01.Domain.Contracts;
using System.Collections.Generic;

namespace SchoolV01.Domain.Entities.GeneralSettings
{
    public class City : AuditableEntity<int>
    {
        public string NameAr { get; set; } = default;

        public string NameEn { get; set; } = default;
        public int CountryId { get; set; }
        public Country Country { get; set; }
        
        public bool IsActive { get; set; } = true;

    }
}
