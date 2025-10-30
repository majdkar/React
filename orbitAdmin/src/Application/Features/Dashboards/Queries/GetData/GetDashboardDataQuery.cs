using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services.Identity;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

using SchoolV01.Domain.Enums;
using System.Linq.Dynamic.Core;

namespace SchoolV01.Application.Features.Dashboards.Queries.GetData
{
    public record GetDashboardDataQuery(int SeasonId) : IRequest<Result<DashboardDataResponse>>;

    internal class GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<GetDashboardDataQueryHandler> localizer)
        : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork = unitOfWork;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer = localizer;

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var response = new DashboardDataResponse
            {
           
            };

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}