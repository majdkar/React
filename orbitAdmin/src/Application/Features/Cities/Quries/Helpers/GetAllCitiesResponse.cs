using System.Collections.Generic;

namespace SchoolV01.Application.Features.Cities.Queries
{
    public class GetAllCitiesResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int CountryId { get; set; }
        public string CountryAr { get; set; }
        public string CountryEn { get; set; }
        public bool IsActive { get; set; }
    }
}
