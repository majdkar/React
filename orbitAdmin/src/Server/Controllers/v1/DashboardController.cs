using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Dashboards.Queries.GetData;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.Extensions.Logging;
using MediatR;
using SchoolV01.Shared.Wrapper;
using System;

namespace SchoolV01.Server.Controllers.v1
{
    [ApiController]
    public class DashboardController : BaseApiController<DashboardController>
    {
        /// <summary>
        /// Get Dashboard Data
        /// </summary>
        /// <returns>Status 200 OK </returns>
        [Authorize(Policy = Permissions.Dashboards.View)]
        [HttpGet("{seasonId}")]
        public async Task<IActionResult> GetDataAsync(int seasonId)
        {
            var result = await Mediator.Send(new GetDashboardDataQuery(seasonId));
            return Ok(result);
        }

    }
}
