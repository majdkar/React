using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.Cities.Queries
{
    public class GetAllCitiesQuery : IRequest<Result<List<GetAllCitiesResponse>>>
    {
        public GetAllCitiesQuery()
        {
        }
    }
    internal class GetAllCitiesCachedQueryHandler : IRequestHandler<GetAllCitiesQuery, Result<List<GetAllCitiesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _cache;

        public GetAllCitiesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCitiesResponse>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<City, GetAllCitiesResponse>> expression = e => new GetAllCitiesResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                CountryId = e.CountryId,
                CountryAr = e.Country.NameAr,
                CountryEn = e.Country.NameEn,
                IsActive = e.IsActive,
            };

            Func<Task<List<GetAllCitiesResponse>>> getAllCities = () => _unitOfWork.Repository<City>().Entities
            .OrderByDescending(e => e.Id)
            .Select(expression)
            .ToListAsync();

            var CityList = await _cache.GetOrAddAsync(nameof(City), getAllCities);

            return await Result<List<GetAllCitiesResponse>>.SuccessAsync(CityList);
        }
    }
}
