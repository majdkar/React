using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Countries.Commands;
using SchoolV01.Application.Features.Countries.Queries;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public interface ICountryManager : IManager
    {
        Task<IResult<List<GetAllCountriesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditCountryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}