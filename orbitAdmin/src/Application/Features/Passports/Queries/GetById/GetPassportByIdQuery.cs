using System;
using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Passports.Queries
{
    public class GetPassportByIdQuery : IRequest<Result<GetPassportByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetPassportByIdQueryHandler : IRequestHandler<GetPassportByIdQuery, Result<GetPassportByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPassportByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetPassportByIdResponse>> Handle(GetPassportByIdQuery query, CancellationToken cancellationToken)
        {
            var passport = await _unitOfWork.Repository<Passport>().GetByIdAsync(query.Id);
            var mappedPassport = _mapper.Map<GetPassportByIdResponse>(passport);
            return await Result<GetPassportByIdResponse>.SuccessAsync(mappedPassport);
        }
    }
}