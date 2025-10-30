using AutoMapper;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Domain.Entities.Identity;

namespace SchoolV01.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, BlazorHeroRole>().ReverseMap();
        }
    }
}