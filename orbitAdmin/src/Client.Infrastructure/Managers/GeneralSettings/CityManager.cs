using SchoolV01.Application.Features.Cities.Commands;
using SchoolV01.Application.Features.Cities.Queries;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public class CityManager : ICityManager
    {
        private readonly HttpClient _httpClient;

        public CityManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CitiesEndpoints.Endpoints + "/export"
                : Routes.CitiesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CitiesEndpoints.Endpoints}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCitiesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CitiesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCitiesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCityCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CitiesEndpoints.Endpoints, request);
            return await response.ToResult<int>();
        }
    }
}