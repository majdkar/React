using AutoMapper;
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
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.Countries.Queries
{
    public class GetAllCountriesQuery : IRequest<Result<List<GetAllCountriesResponse>>>
    {
        public GetAllCountriesQuery()
        {
        }
    }
    internal class GetAllCountriesCachedQueryHandler : IRequestHandler<GetAllCountriesQuery, Result<List<GetAllCountriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCountriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCountriesResponse>>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            var isArabic = CultureInfo.CurrentCulture.Name.Contains("ar");
            Func<Task<List<Country>>> getAllCountries = () => _unitOfWork.Repository<Country>().Entities
            .OrderBy(x => isArabic ? x.NameAr : x.NameEn)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            var CountryList = await _cache.GetOrAddAsync(nameof(Country) + $"-{isArabic}", getAllCountries);
            var mappedCountries = _mapper.Map<List<GetAllCountriesResponse>>(CountryList);
            return await Result<List<GetAllCountriesResponse>>.SuccessAsync(mappedCountries);
        }
    }
}
