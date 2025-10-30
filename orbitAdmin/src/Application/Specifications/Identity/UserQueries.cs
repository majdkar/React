using SchoolV01.Application.Requests.Identity;
using SchoolV01.Domain.Entities.Identity;

namespace SchoolV01.Application.Specifications.Identity
{
    public static class UserQueries
    {
        public static RegisterRequest ToRequest(this BlazorHeroUser source)
        {
            return new RegisterRequest
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                HomePhoneNumber = source.HomePhoneNumber,
                Address = source.Address,
                PictureUrl = source.PictureUrl

            };
        }
    }
}
