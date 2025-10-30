using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
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
    public partial class AddEditPassportCommand : IRequest<Result<int>>
    {
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
        public string Description { get; set; }
		

        //[Required]
        //public decimal Tax { get; set; }
    }

    internal class AddEditPassportCommandHandler : IRequestHandler<AddEditPassportCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditPassportCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditPassportCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPassportCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPassportCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var passport = _mapper.Map<Passport>(command);
                await _unitOfWork.Repository<Passport>().AddAsync(passport);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPassportsCacheKey);
                return await Result<int>.SuccessAsync(passport.Id, _localizer["Passport Saved"]);
            }
            else
            {
                var passport = await _unitOfWork.Repository<Passport>().GetByIdAsync(command.Id);
                if (passport != null)
                {
					
                    await _unitOfWork.Repository<Passport>().UpdateAsync(passport);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPassportsCacheKey);
                    return await Result<int>.SuccessAsync(passport.Id, _localizer["Passport Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Passport Not Found!"]);
                }
            }
        }
    }
}