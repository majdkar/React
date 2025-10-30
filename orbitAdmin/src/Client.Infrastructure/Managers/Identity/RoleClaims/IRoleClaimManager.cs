using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}