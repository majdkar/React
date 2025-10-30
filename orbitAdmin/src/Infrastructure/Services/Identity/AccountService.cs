using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Interfaces.Services.Account;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Domain.Entities.Identity;
using System;

namespace SchoolV01.Infrastructure.Services.Identity
{
    public class AccountService(
        UserManager<BlazorHeroUser> userManager,
        IUploadService uploadService,
        IStringLocalizer<AccountService> localizer) : IAccountService
    {
        private readonly UserManager<BlazorHeroUser> _userManager = userManager;
        private readonly IUploadService _uploadService = uploadService;
        private readonly IStringLocalizer<AccountService> _localizer = localizer;

        public async Task<IResult> ResetPasswordAndEmailAsync(ResetPasswordAndEmailRequest request, string userId, bool isAdmin)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (request.OldEmail != null)
                user = await _userManager.FindByEmailAsync(request.OldEmail);

            if (user == null || (user?.Id != userId && !isAdmin))
                return await Result.FailAsync(_localizer["User Not Found."]);

            var newUserWithSameEmail = await _userManager.FindByEmailAsync(request.NewEmail);

            if (newUserWithSameEmail == null || newUserWithSameEmail?.Id == userId || (isAdmin && newUserWithSameEmail == user))
            {
                var identityResult = new IdentityResult();
                if (user.Email != request.NewEmail)
                {
                    user.Email = request.NewEmail;
                    user.UserName = request.NewEmail;

                    identityResult = await _userManager.UpdateAsync(user);
                }
                if (request.Password != null)
                {
                    identityResult = await _userManager.RemovePasswordAsync(user);
                    identityResult = await _userManager.AddPasswordAsync(user, request.Password);
                }
                var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
                //  await _signInManager.RefreshSignInAsync(user);
                return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
            }
            else
            {
                return await Result.FailAsync(string.Format(_localizer["Email {0} is already used."], request.NewEmail));
            }
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result.FailAsync(_localizer["User Not Found."]);
            }

            var identityResult = await _userManager.ChangePasswordAsync(
                user,
                model.Password,
                model.NewPassword);
            var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest request, string userId)
        {
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    return await Result.FailAsync(string.Format(_localizer["Phone number {0} is already used."], request.PhoneNumber));
                }
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null || userWithSameEmail.Id == userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return await Result.FailAsync(_localizer["User Not Found."]);
                }
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (request.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
                }
                var identityResult = await _userManager.UpdateAsync(user);
                var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
                //await _signInManager.RefreshSignInAsync(user);
                return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
            }
            else
            {
                return await Result.FailAsync(string.Format(_localizer["Email {0} is already used."], request.Email));
            }
        }

        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result<string>.FailAsync(_localizer["User Not Found"]);
            }
            return await Result<string>.SuccessAsync(data: user.PictureUrl);
        }

        public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return await Result<string>.FailAsync(message: _localizer["User Not Found"]);
            var filePath = _uploadService.UploadAsync(request);
            user.PictureUrl = filePath;
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
            return identityResult.Succeeded ? await Result<string>.SuccessAsync(data: filePath) : await Result<string>.FailAsync(errors);
        }

        public async Task<IResult> UpdateProfileByAdminAsync(UpdateProfileByAdminRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithSamePhoneNumber != null && userWithSamePhoneNumber.Id != request.UserId)
                {
                    return await Result.FailAsync(string.Format(_localizer["Phone number {0} is already used."], request.PhoneNumber));
                }
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null || userWithSameEmail.Id == request.UserId)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return await Result.FailAsync(_localizer["User Not Found."]);
                }
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (request.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
                }

                var email = await _userManager.GetEmailAsync(user);
                if (request.Email != email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, request.Email);
                }
                var identityResult = await _userManager.UpdateAsync(user);
                var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
                //await _signInManager.RefreshSignInAsync(user);
                return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
            }
            else
            {
                return await Result.FailAsync(string.Format(_localizer["Email {0} is already used."], request.Email));
            }
        }


        public async Task<IResult> ChangePasswordByAdminAsync(ChangePasswordByAdminRequest model)
        {
            var user = await this._userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return await Result.FailAsync(_localizer["User Not Found."]);
            }

            var passwordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            user.PasswordHash = passwordHash;
            user.LastModifiedOn = DateTime.Now.Date;
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }


    }
}