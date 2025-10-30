using SchoolV01.Application.Interfaces.Common;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Shared.Wrapper;
using System.Threading.Tasks;

namespace SchoolV01.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);
        Task<IResult> ResetPasswordAndEmailAsync(ResetPasswordAndEmailRequest model, string userId, bool isAdmin);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);

        Task<IResult> UpdateProfileByAdminAsync(UpdateProfileByAdminRequest model);

        Task<IResult> ChangePasswordByAdminAsync(ChangePasswordByAdminRequest model);
    }
}