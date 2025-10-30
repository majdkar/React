using AutoMapper;
using SchoolV01.Infrastructure.Models.Audit;
using SchoolV01.Application.Responses.Audit;

namespace SchoolV01.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}