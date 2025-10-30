using SchoolV01.Domain.Contracts;

namespace SchoolV01.Domain.Entities.GeneralSettings
{
    public class Language : AuditableEntity<int>
    {
        
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public bool IsActive { get; set; }
    }
}
