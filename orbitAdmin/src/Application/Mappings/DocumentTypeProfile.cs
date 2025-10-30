using AutoMapper;
using SchoolV01.Application.Features.DocumentTypes.Commands;
using SchoolV01.Application.Features.DocumentTypes.Queries;
using SchoolV01.Domain.Entities.Misc;

namespace SchoolV01.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}