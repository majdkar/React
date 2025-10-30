using SchoolV01.Domain.Contracts;
using System.Collections.Generic;

namespace SchoolV01.Domain.Entities.GeneralSettings
{
    public class Country : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public string PhoneCode { get; set; }
        public int Order { get; set; } = 250;
        public bool IsActive { get; set; } = true;
    }
}