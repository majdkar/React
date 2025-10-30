using SchoolV01.Application.Requests.Identity;
using SchoolV01.Shared.Wrapper;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Identity.Account
{
    public interface IAccountManager : IManager
    {
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model);
        Task<IResult> ChangePasswordByAdminAsync(ChangePasswordByAdminRequest model);
        Task<IResult> ResetPasswordAndEmailAsync(ResetPasswordAndEmailRequest model);

        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model);
        Task<IResult> UpdateProfileByAdminAsync(UpdateProfileByAdminRequest model);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}