using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Interfaces.Services.Account;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Shared.Constants.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.Identity
{
    [Authorize]
    [Route("api/identity/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICurrentUserService _currentUser;

        public AccountController(IAccountService accountService, ICurrentUserService currentUser)
        {
            _accountService = accountService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPut(nameof(UpdateProfile))]
        public async Task<ActionResult> UpdateProfile(UpdateProfileRequest model)
        {
            var response = await _accountService.UpdateProfileAsync(model, _currentUser.UserId);
            return Ok(response);
        }
        /// <summary>
        /// Reset Password And Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPut(nameof(ResetPasswordAndEmail))]
        public async Task<ActionResult> ResetPasswordAndEmail(ResetPasswordAndEmailRequest model)
        {
            var _isAdmin = HttpContext.User.IsInRole(RoleConstants.AdministratorRole);
            var response = await _accountService.ResetPasswordAndEmailAsync(model, _currentUser.UserId, _isAdmin);
            return Ok(response);
        }
        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPut(nameof(ChangePassword))]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequest model)
        {
            var response = await _accountService.ChangePasswordAsync(model, _currentUser.UserId);
            return Ok(response);
        }

        /// <summary>
        /// Get Profile picture by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Status 200 OK </returns>
        [HttpGet("profile-picture/{userId}")]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        public async Task<IActionResult> GetProfilePictureAsync(string userId)
        {
            return Ok(await _accountService.GetProfilePictureAsync(userId));
        }

        /// <summary>
        /// Update Profile Picture
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("profile-picture/{userId}")]
        public async Task<IActionResult> UpdateProfilePictureAsync(UpdateProfilePictureRequest request)
        {
            return Ok(await _accountService.UpdateProfilePictureAsync(request, _currentUser.UserId));
        }



        /// <summary>
        /// Update Profile By Admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPut(nameof(UpdateProfileByAdmin))]
        [Authorize(Roles = "Administrator,Basic")]
        public async Task<ActionResult> UpdateProfileByAdmin(UpdateProfileByAdminRequest model)
        {
            var response = await _accountService.UpdateProfileByAdminAsync(model);
            return Ok(response);
        }

        /// <summary>
        /// Change Password By Admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPut(nameof(ChangePasswordByAdmin))]
        [Authorize(Roles = "Administrator,Basic")]
        public async Task<ActionResult> ChangePasswordByAdmin(ChangePasswordByAdminRequest model)
        {
            var response = await _accountService.ChangePasswordByAdminAsync(model);
            return Ok(response);
        }
    }
}