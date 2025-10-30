using System;
using SchoolV01.Application.Features.Passports.Queries;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Passports.Commands;

namespace SchoolV01.Client.Infrastructure.Managers.OwnersManagement.Passport
{
    public class PassportManager : IPassportManager
    {
        private readonly HttpClient _httpClient;

        public PassportManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PassportsEndpoints.Export
                : Routes.PassportsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PassportsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllPassportsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PassportsEndpoints.GetAll);
            return await response.ToResult<List<GetAllPassportsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPassportCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PassportsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}