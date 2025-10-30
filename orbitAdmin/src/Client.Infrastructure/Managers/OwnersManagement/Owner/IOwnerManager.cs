using System;
using SchoolV01.Application.Features.Owners.Commands;
using SchoolV01.Application.Requests.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using SchoolV01.Application.Features.Owners.Queries;

namespace SchoolV01.Client.Infrastructure.Managers.OwnersManagement.Owner
{
    public interface IOwnerManager : IManager
    {
        Task<IResult<List<GetAllPagedOwnersResponse>>> GetAllAsync();
        Task<PaginatedResult<GetAllPagedOwnersResponse>> GetOwnersAsync(GetAllPagedOwnersRequest request);

        Task<IResult<string>> GetOwnerImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditOwnerCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}