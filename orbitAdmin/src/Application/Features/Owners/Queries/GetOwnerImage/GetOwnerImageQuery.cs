using System;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Owners.Queries.GetOwnerImage
{
    public class GetOwnerImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetOwnerImageQuery(int ownerId)
        {
            Id = ownerId;
        }
    }

    internal class GetOwnerImageQueryHandler : IRequestHandler<GetOwnerImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetOwnerImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetOwnerImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Owner>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}