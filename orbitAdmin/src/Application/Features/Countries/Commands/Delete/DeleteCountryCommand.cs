using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.Countries.Commands
{
    public class DeleteCountryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteCountryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteCountryCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCountryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCountryCommand command, CancellationToken cancellationToken)
        {
            var Country = await _unitOfWork.Repository<Country>().Entities.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (Country != null)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(Country);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(Country));
                return await Result<int>.SuccessAsync(Country.Id, _localizer["Country Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Country Not Found!"]);
            }
        }
    }
}
