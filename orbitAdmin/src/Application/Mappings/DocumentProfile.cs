using AutoMapper;
using SchoolV01.Application.Features.Documents.Commands;
using SchoolV01.Application.Features.Documents.Queries;
using SchoolV01.Domain.Entities.Misc;

namespace SchoolV01.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}