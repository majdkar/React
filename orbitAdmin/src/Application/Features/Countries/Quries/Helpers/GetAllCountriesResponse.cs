
namespace SchoolV01.Application.Features.Countries.Queries
{
    public class GetAllCountriesResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public string PhoneCode { get; set; }
        public bool IsActive { get; set; }

    }
}
