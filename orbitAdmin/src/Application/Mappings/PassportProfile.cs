using System;
using AutoMapper;
using SchoolV01.Application.Features.Passports.Commands;
using SchoolV01.Application.Features.Passports.Queries;
using SchoolV01.Application.Features.Passports.Queries;
using SchoolV01.Domain.Entities.OwnersManagement;

namespace SchoolV01.Application.Mappings
{
    public class PassportProfile : Profile
    {
        public PassportProfile()
        {
            CreateMap<AddEditPassportCommand, Passport>().ReverseMap();
            CreateMap<GetPassportByIdResponse, Passport>().ReverseMap();
            CreateMap<GetAllPassportsResponse, Passport>().ReverseMap();
        }
    }
}