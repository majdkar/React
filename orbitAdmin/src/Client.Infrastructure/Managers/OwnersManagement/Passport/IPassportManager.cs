using System;
using SchoolV01.Application.Features.Passports.Queries;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Passports.Commands;

namespace SchoolV01.Client.Infrastructure.Managers.OwnersManagement.Passport
{
    public interface IPassportManager : IManager
    {
        Task<IResult<List<GetAllPassportsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditPassportCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}