using SchoolV01.Application.Interfaces.Common;
using System.Security.Claims;

namespace SchoolV01.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
        ClaimsPrincipal User { get; }
    }
}