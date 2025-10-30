using AutoMapper;
using SchoolV01.Domain.Interfaces.Chat;
using SchoolV01.Domain.Models.Chat;
using SchoolV01.Domain.Entities.Identity;

namespace SchoolV01.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<BlazorHeroUser>>().ReverseMap();
        }
    }
}