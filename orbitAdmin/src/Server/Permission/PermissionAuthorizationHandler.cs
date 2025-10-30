using SchoolV01.Shared.Constants.Permission;
using SchoolV01.Shared.Constants.Role;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolV01.Server.Permission
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        { }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                await Task.CompletedTask;
            }
            if (context.User.IsInRole(RoleConstants.AdministratorRole))
            {
                context.Succeed(requirement);
                await Task.CompletedTask;

            }
            var permissions = context.User.Claims.Where(x => x.Type == ApplicationClaimTypes.Permission &&
                                                                 x.Value == requirement.Permission &&
                                                                x.Issuer == "LOCAL AUTHORITY");
            if (permissions.Any())
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
        }
    }
}