using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using SchoolV01.Application.Exceptions;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Interfaces.Services.Identity;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Application.Requests.Mail;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Infrastructure.Specifications;
using SchoolV01.Shared.Constants.Role;
using SchoolV01.Shared.Wrapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Shared.Constants.User;

namespace SchoolV01.Infrastructure.Services.Identity
{
    public class UserService(UserManager<BlazorHeroUser> userManager, IMapper mapper, RoleManager<BlazorHeroRole> roleManager, IMailService mailService, IStringLocalizer<UserService> localizer, IExcelService excelService, ICurrentUserService currentUserService, IUnitOfWork<int> unitOfWork, IUploadService uploadService)
        : IUserService
    {
        private readonly UserManager<BlazorHeroUser> _userManager = userManager;
        private readonly RoleManager<BlazorHeroRole> _roleManager = roleManager;
        private readonly IMailService _mailService = mailService;
        private readonly IStringLocalizer<UserService> _localizer = localizer;
        private readonly IExcelService _excelService = excelService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork<int> _unitOfWork = unitOfWork;
        private readonly IUploadService _uploadService = uploadService;

        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userManager.Users.OrderByDescending(x => x.CreatedOn).ToListAsync();
            var adminUsers = await _userManager.GetUsersInRoleAsync(RoleConstants.AdministratorRole);
            var result = _mapper.Map<List<UserResponse>>(users.Where(x => !adminUsers.Contains(x)));
            return await Result<List<UserResponse>>.SuccessAsync(result);
        }

        public async Task<IResult> DeleteAsync(BlazorHeroUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return await Result.FailAsync(result.Errors.Select(x => x.Description).ToList());

            return await Result.SuccessAsync();
        }
        public async Task<Result<BlazorHeroUser>> RegisterAsync(RegisterRequest request, string origin)
        {

            bool canCreate = false;
            if (request.ClientType == RoleConstants.AdministratorRole)
                canCreate = await _userManager.FindByNameAsync(request.UserName ?? request.Email) is null;
            else
            {
                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    var userWithSamePhoneNumber = await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);
                    if (userWithSamePhoneNumber)
                        return await Result<BlazorHeroUser>.FailAsync(string.Format(_localizer["Phone number {0} is already registered."], request.PhoneNumber));
                }
                try
                {
                    var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
                    var userWithSameUserName = await _userManager.FindByNameAsync(request.Email);
                    canCreate = userWithSameUserName is null && (userWithSameEmail is null || userWithSameEmail?.ClientType == RoleConstants.AdministratorRole);
                }
                catch (Exception)
                {
                    canCreate = await _userManager.FindByNameAsync(request.Email) is null;
                }
                request.UserName = request.Email;
            }
            if (canCreate)
            {
                if (request.UploadRequest != null)
                {
                    request.UploadRequest.FileName = $"P-{new Random(9)}{request.UploadRequest.Extension}";
                    request.PictureUrl = _uploadService.UploadAsync(request.UploadRequest);
                }
                var user = new BlazorHeroUser
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName ?? request.Email,
                    PhoneNumber = request.PhoneNumber,
                    HomePhoneNumber = request.HomePhoneNumber,
                    ClientType = request.ClientType,
                    PictureUrl = request.PictureUrl,
                    Address = request.Address,
                    IsActive = request.IsActive,
                    EmailConfirmed = true,// request.AutoConfirmEmail
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, request.ClientType ?? RoleConstants.BasicRole);
                    //if (!request.AutoConfirmEmail)
                    //{
                    //    var verificationUri = await SendVerificationEmail(user, origin);
                    //    var mailRequest = new MailRequest
                    //    {
                    //        From = "mail@confirmpassword.com",
                    //        To = user.Email,
                    //        Body = string.Format(_localizer["Please confirm your account by <a href='{0}'>clicking here</a>."], verificationUri),
                    //        Subject = _localizer["Confirm Registration"]
                    //    };
                    //    BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                    //    return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["User {0} Registered. Please check your Mailbox to verify!"], user.UserName));
                    //}
                    return await Result<BlazorHeroUser>.SuccessAsync(user, user.Id);
                }
                else
                {
                    return await Result<BlazorHeroUser>.FailAsync(result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(request.UserName) && request.ClientType == RoleConstants.AdministratorRole)
                    return await Result<BlazorHeroUser>.FailAsync(string.Format(_localizer["User Name {0} is already registered."], request.UserName));
                else
                    return await Result<BlazorHeroUser>.FailAsync(string.Format(_localizer["Email {0} is already registered."], request.Email));
            }
        }

        //Register Team Advance User

        private async Task<string> SendVerificationEmail(BlazorHeroUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/identity/user/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var result = _mapper.Map<UserResponse>(user);
            return await Result<UserResponse>.SuccessAsync(result);
        }

        public async Task<IResult<UserResponse>> GetByEmailAsync(string userEmail)
        {
            var user = await _userManager.Users.Where(u => u.Email == userEmail).FirstOrDefaultAsync();
            if (user is null)
                return await Result<UserResponse>.FailAsync(_localizer["User Not Found"]);

            var result = _mapper.Map<UserResponse>(user);
            return await Result<UserResponse>.SuccessAsync(result);
        }

        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
            if (isAdmin)
            {
                return await Result.FailAsync(_localizer["Administrators Profile's Status cannot be toggled"]);
            }
            if (user != null)
            {
                user.IsActive = request.ActivateUser;
                var identityResult = await _userManager.UpdateAsync(user);
            }
            return await Result.SuccessAsync();
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var viewModel = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                var userRolesViewModel = new UserRoleModel
                {
                    RoleName = role.Name,
                    RoleDescription = role.Description
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var result = new UserRolesResponse { UserRoles = viewModel };
            return await Result<UserRolesResponse>.SuccessAsync(result);
        }

        public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user.Email == "admin@example.com")
            {
                return await Result.FailAsync(_localizer["Not Allowed."]);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
            {
                var tryToAddAdministratorRole = selectedRoles
                    .Any(x => x.RoleName == RoleConstants.AdministratorRole);
                var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
                if (tryToAddAdministratorRole && !userHasAdministratorRole || !tryToAddAdministratorRole && userHasAdministratorRole)
                {
                    return await Result.FailAsync(_localizer["Not Allowed to add or delete Administrator Role if you have not this role."]);
                }
            }

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));
            return await Result.SuccessAsync(_localizer["Roles Updated"]);
        }

        public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT."], user.Email));
            }
            else
            {
                throw new ApiException(string.Format(_localizer["An error occurred while confirming {0}"], user.Email));
            }
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByNameAsync(request.EmailOrUserName);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return await Result.FailAsync(_localizer["An Error has occurred!"]);
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            var mailRequest = new MailRequest
            {
                Body = string.Format(_localizer["Please reset your password by <a href='{0}>clicking here</a>."], HtmlEncoder.Default.Encode(passwordResetURL)),
                Subject = _localizer["Reset Password"],
                To = request.EmailOrUserName
            };
            BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
            return await Result.SuccessAsync(_localizer["Password Reset Mail has been sent to your authorized Email."]);
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.EmailOrUserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync(_localizer["Password Reset Successful!"]);
            }
            else
            {
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }
        }

        public async Task<IResult> ResetPasswordUserAsync(ResetPasswordUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.EmailOrUserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }
            var result = await _userManager.RemovePasswordAsync(user);
            result = await _userManager.AddPasswordAsync(user, request.Password);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync(_localizer["Password Reset Successful!"]);
            }
            else
            {
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _userManager.Users.CountAsync();
            return count;
        }

        public async Task<string> ExportToExcelAsync(string searchString = "")
        {
            var userSpec = new UserFilterSpecification(searchString);
            var users = await _userManager.Users
                .Specify(userSpec)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync();
            var result = await _excelService.ExportAsync(users, sheetName: _localizer["Users"],
                mappers: new Dictionary<string, Func<BlazorHeroUser, object>>
                {
                    { _localizer["Id"], item => item.Id },
                    { _localizer["FirstName"], item => item.FirstName },
                    { _localizer["LastName"], item => item.LastName },
                    { _localizer["UserName"], item => item.UserName },
                    { _localizer["Email"], item => item.Email },
                    { _localizer["EmailConfirmed"], item => item.EmailConfirmed },
                    { _localizer["PhoneNumber"], item => item.PhoneNumber },
                    { _localizer["PhoneNumberConfirmed"], item => item.PhoneNumberConfirmed },
                    { _localizer["IsActive"], item => item.IsActive },
                    { _localizer["CreatedOn (Local)"], item => DateTime.SpecifyKind(item.CreatedOn, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["CreatedOn (UTC)"], item => item.CreatedOn.ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["ProfilePictureDataUrl"], item => item.PictureUrl },
                });

            return result;
        }

        public async Task<Result<List<UserResponse>>> GetAllByRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var result = _mapper.Map<List<UserResponse>>(users.Where(x => x.Email != UserConstants.DefaultEmail));
            return await Result<List<UserResponse>>.SuccessAsync(result);
        }
    }
}