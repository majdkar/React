using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System;
namespace SchoolV01.Application.Features.Owners.Commands
{
    public class DeleteOwnerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteOwnerCommandHandler> _localizer;

        public DeleteOwnerCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteOwnerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteOwnerCommand command, CancellationToken cancellationToken)
        {
            var owner = await _unitOfWork.Repository<Owner>().GetByIdAsync(command.Id);
            if (owner != null)
            {
                await _unitOfWork.Repository<Owner>().DeleteAsync(owner);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(owner.Id, _localizer["Owner Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Owner Not Found!"]);
            }
        }
    }
}