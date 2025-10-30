using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Common;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}