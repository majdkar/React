using AutoMapper;
using SchoolV01.Application.Features.Countries.Commands;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<AddEditCountryCommand, Country>().ReverseMap();
            CreateMap<GetAllCountriesResponse, Country>().ReverseMap();
        }
    }
}
