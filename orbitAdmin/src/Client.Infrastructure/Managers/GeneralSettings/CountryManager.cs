using SchoolV01.Application.Features.Countries.Commands;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public class CountryManager : ICountryManager
    {
        private readonly HttpClient _httpClient;

        public CountryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.CountriesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CountriesEndpoints.Endpoints}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCountriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CountriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCountriesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCountryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CountriesEndpoints.Endpoints, request);
            return await response.ToResult<int>();
        }
    }
}