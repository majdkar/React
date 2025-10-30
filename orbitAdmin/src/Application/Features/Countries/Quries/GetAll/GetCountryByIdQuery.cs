using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Countries.Queries
{
    public class GetCountryByIdQuery : IRequest<Result<GetAllCountriesResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Result<GetAllCountriesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllCountriesResponse>> Handle(GetCountryByIdQuery query, CancellationToken cancellationToken)
        {
            var Country = await _unitOfWork.Repository<Country>().GetByIdAsync(query.Id);
            var mappedCountry = _mapper.Map<GetAllCountriesResponse>(Country);
            return await Result<GetAllCountriesResponse>.SuccessAsync(mappedCountry);
        }
    }
}
