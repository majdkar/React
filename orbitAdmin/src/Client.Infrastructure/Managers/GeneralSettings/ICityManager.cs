using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Cities.Commands;
using SchoolV01.Application.Features.Cities.Queries;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public interface ICityManager : IManager
    {
        Task<IResult<List<GetAllCitiesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditCityCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}