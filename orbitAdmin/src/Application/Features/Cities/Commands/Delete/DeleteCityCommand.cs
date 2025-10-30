using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Cities.Commands
{
    public class DeleteCityCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteCityCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteCityCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCityCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCityCommand command, CancellationToken cancellationToken)
        {
            var isCityUsed = false;// await _productRepository.IsCityUsed(command.Id);
            if (!isCityUsed)
            {
                var City = await _unitOfWork.Repository<City>().GetByIdAsync(command.Id);
                if (City != null)
                {
                    await _unitOfWork.Repository<City>().DeleteAsync(City);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(City));
                    return await Result<int>.SuccessAsync(City.Id, _localizer["City Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["City Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
