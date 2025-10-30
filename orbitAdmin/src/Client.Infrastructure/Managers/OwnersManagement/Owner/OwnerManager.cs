using System;
using SchoolV01.Application.Features.Owners.Commands;
using SchoolV01.Application.Requests.OwnersManagement;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using SchoolV01.Application.Features.Owners.Queries;

namespace SchoolV01.Client.Infrastructure.Managers.OwnersManagement.Owner
{
    public class OwnerManager : IOwnerManager
    {
        private readonly HttpClient _httpClient;

        public OwnerManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<List<GetAllPagedOwnersResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PassportsEndpoints.GetAll);
            return await response.ToResult<List<GetAllPagedOwnersResponse>>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.OwnersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.OwnersEndpoints.Export
                : Routes.OwnersEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetOwnerImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.OwnersEndpoints.GetOwnerImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedOwnersResponse>> GetOwnersAsync(GetAllPagedOwnersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.OwnersEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedOwnersResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditOwnerCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.OwnersEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}