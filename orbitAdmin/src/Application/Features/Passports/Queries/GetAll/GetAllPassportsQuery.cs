using System;
using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using LazyCache;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Passports.Queries
{
    public class GetAllPassportsQuery : IRequest<Result<List<GetAllPassportsResponse>>>
    {
        public GetAllPassportsQuery()
        {
        }
    }

    internal class GetAllPassportsCachedQueryHandler : IRequestHandler<GetAllPassportsQuery, Result<List<GetAllPassportsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllPassportsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPassportsResponse>>> Handle(GetAllPassportsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Passport>>> getAllPassports = () => _unitOfWork.Repository<Passport>().GetAllAsync();
            var passportList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPassportsCacheKey, getAllPassports);
            var mappedPassports = _mapper.Map<List<GetAllPassportsResponse>>(passportList);
            return await Result<List<GetAllPassportsResponse>>.SuccessAsync(mappedPassports);
        }
    }
}