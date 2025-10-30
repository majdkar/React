using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Net.Http;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Dashboards.Queries.GetData;
using System.Collections.Generic;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Application.Features.Dashboards.Dtos;
using System;

namespace SchoolV01.Client.Infrastructure.Managers.Dashboard
{
    public class DashboardManager : IDashboardManager
    {
        private readonly HttpClient _httpClient;

        public DashboardManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<DailyScheduleResponse>>> GetDailySchedule(DayOfWeek day, int seasonId)
        {
            var response = await _httpClient.GetAsync($"{Routes.DashboardEndpoints.GetDailySchedule}/{day}/{seasonId}");
            return await response.ToResult<List<DailyScheduleResponse>>();
        }

        public async Task<IResult<DashboardDataResponse>> GetDataAsync(int seasonId)
        {
            var response = await _httpClient.GetAsync(Routes.DashboardEndpoints.GetData+$"/{seasonId}");
            var data = await response.ToResult<DashboardDataResponse>();
            return data;
        }

        public Task<List<NotificationResponse>> GetNotificationsAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

    }
}