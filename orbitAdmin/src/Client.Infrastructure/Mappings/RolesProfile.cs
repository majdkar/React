using AutoMapper;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Application.Responses.Identity;

namespace SchoolV01.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
        }
    }
}