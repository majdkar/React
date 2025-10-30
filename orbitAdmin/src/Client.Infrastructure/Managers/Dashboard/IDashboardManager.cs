using SchoolV01.Shared.Wrapper;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Dashboards.Queries.GetData;
using SchoolV01.Application.Responses.Identity;
using System.Collections.Generic;
using SchoolV01.Application.Features.Dashboards.Dtos;
using System;

namespace SchoolV01.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<List<NotificationResponse>> GetNotificationsAsync(string userId);
        Task<IResult<DashboardDataResponse>> GetDataAsync(int seasonId);
        Task<IResult<List<DailyScheduleResponse>>> GetDailySchedule(DayOfWeek day, int seasonId);
    }
}