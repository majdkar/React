using System;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using SchoolV01.Shared.Constants.Application;

namespace SchoolV01.Application.Features.Passports.Commands
{
    public class DeletePassportCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePassportCommandHandler : IRequestHandler<DeletePassportCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeletePassportCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePassportCommandHandler(IUnitOfWork<int> unitOfWork,  IStringLocalizer<DeletePassportCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePassportCommand command, CancellationToken cancellationToken)
        {
            var isPassportUsed = false;// await _productRepository.IsPassportUsed(command.Id);
            if (!isPassportUsed)
            {
                var passport = await _unitOfWork.Repository<Passport>().GetByIdAsync(command.Id);
                if (passport != null)
                {
                    await _unitOfWork.Repository<Passport>().DeleteAsync(passport);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPassportsCacheKey);
                    return await Result<int>.SuccessAsync(passport.Id, _localizer["Passport Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Passport Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}