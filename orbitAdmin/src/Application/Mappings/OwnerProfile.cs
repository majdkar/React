using System;
using AutoMapper;
using SchoolV01.Application.Features.Owners.Commands;
using SchoolV01.Domain.Entities.OwnersManagement;

namespace SchoolV01.Application.Mappings
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<AddEditOwnerCommand, Owner>().ReverseMap();
        }
    }
}