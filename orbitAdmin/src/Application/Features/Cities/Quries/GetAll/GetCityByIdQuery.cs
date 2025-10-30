using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.Cities.Queries
{
    public class GetCityByIdQuery : IRequest<Result<GetAllCitiesResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, Result<GetAllCitiesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCityByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllCitiesResponse>> Handle(GetCityByIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<City, GetAllCitiesResponse>> expression = e => new GetAllCitiesResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                CountryId = e.CountryId,
                CountryAr = e.Country.NameAr,
                CountryEn = e.Country.NameEn,
            };

            var data = await _unitOfWork.Repository<City>().Entities
             .Select(expression)
             .FirstOrDefaultAsync(x => x.Id == query.Id);

            return await Result<GetAllCitiesResponse>.SuccessAsync(data);
        }
    }
}
