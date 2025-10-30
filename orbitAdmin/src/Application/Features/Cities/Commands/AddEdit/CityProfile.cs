using AutoMapper;
using SchoolV01.Application.Features.Cities.Commands;
using SchoolV01.Application.Features.Cities.Queries;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Mappings
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<AddEditCityCommand, City>().ReverseMap();
            CreateMap<City, GetAllCitiesResponse>()
                .ForMember(dst => dst.CountryAr, opt => opt.MapFrom(src => src.Country.NameAr))
                .ForMember(dst => dst.CountryEn, opt => opt.MapFrom(src => src.Country.NameEn)); ;
        }
    }
}
