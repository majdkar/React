using SchoolV01.Application.Interfaces.Common;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Shared.Constants.Role;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Interfaces.Services.Identity
{
    public interface IUserService : IService
    {
        Task<Result<List<UserResponse>>> GetAllAsync();
        Task<Result<List<UserResponse>>> GetAllByRoleAsync(string roleName);

        Task<int> GetCountAsync();

        Task<IResult<UserResponse>> GetAsync(string userId);
        Task<IResult<UserResponse>> GetByEmailAsync(string userEmail);

        Task<Result<BlazorHeroUser>> RegisterAsync(RegisterRequest request, string origin);
        Task<IResult> DeleteAsync(BlazorHeroUser user);

        Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);

        Task<IResult<UserRolesResponse>> GetRolesAsync(string id);

        Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task<IResult> ResetPasswordUserAsync(ResetPasswordUserRequest request);

        Task<string> ExportToExcelAsync(string searchString = "");
    }
}