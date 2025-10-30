using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Countries.Commands
{
    public class AddEditCountryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }

        public string NameEn { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public string PhoneCode { get; set; }
        public bool IsActive { get; set; }
    }

    internal class AddEditCountryCommandHandler : IRequestHandler<AddEditCountryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCountryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditCountryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCountryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCountryCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var country = _mapper.Map<Country>(command);
                await _unitOfWork.Repository<Country>().AddAsync(country);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(Country));
                return await Result<int>.SuccessAsync(country.Id, _localizer["Country Saved"]);
            }
            else
            {
                var country = await _unitOfWork.Repository<Country>().GetByIdAsync(command.Id);
                if (country != null)
                {
                    country.NameAr = command.NameAr ?? country.NameAr;
                    country.NameEn = command.NameEn ?? country.NameEn;
                    country.PhoneCode = command.PhoneCode ?? country.PhoneCode;
                    country.Alpha2Code = command.Alpha2Code ?? country.Alpha2Code;
                    country.Alpha3Code = command.Alpha3Code ?? country.Alpha3Code;
                    country.IsActive = command.IsActive;

                    await _unitOfWork.Repository<Country>().UpdateAsync(country);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(Country));
                    return await Result<int>.SuccessAsync(country.Id, _localizer["Country Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Country Not Found!"]);
                }
            }
        }
    }
}
