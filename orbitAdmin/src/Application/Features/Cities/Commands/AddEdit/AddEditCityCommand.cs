using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Cities.Commands
{
    public class AddEditCityCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }

        public string NameEn { get; set; }
        public bool IsActive { get; set; } = true;
        public int CountryId { get; set; }
    }

    internal class AddEditCityCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<AddEditCityCommand> localizer) : IRequestHandler<AddEditCityCommand, Result<int>>
    {
        private readonly IStringLocalizer<AddEditCityCommand> _localizer = localizer;
        private readonly IUnitOfWork<int> _unitOfWork = unitOfWork;

        public async Task<Result<int>> Handle(AddEditCityCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var city = _unitOfWork.Map<City>(command);

                await _unitOfWork.Repository<City>().AddAsync(city);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(City));

                return await Result<int>.SuccessAsync(city.Id, _localizer["City Saved"]);
            }
            else
            {
                var city = await _unitOfWork.Repository<City>().GetByIdAsync(command.Id);
                if (city != null)
                {
                    city.NameAr = command.NameAr ?? city.NameAr;
                    city.NameEn = command.NameEn ?? city.NameEn;
                    city.IsActive = command.IsActive;
                    city.CountryId = command.CountryId;

                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(City));

                    return await Result<int>.SuccessAsync(city.Id, _localizer["City Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["City Not Found!"]);
                }
            }
        }
    }
}
