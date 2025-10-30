using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
namespace SchoolV01.Application.Features.Owners.Commands
{
    public partial class AddEditOwnerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
		[Required]
        public string Description { get; set; }
		
		[Required]
		public int PassportId { get; set; }
        //[Required]
        //public string Barcode { get; set; }
        //[Required]
        //public decimal Rate { get; set; }
        //[Required]
        //public int BrandId { get; set; }
		

		public string ImageDataURL { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditOwnerCommandHandler : IRequestHandler<AddEditOwnerCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditOwnerCommandHandler> _localizer;

        public AddEditOwnerCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditOwnerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditOwnerCommand command, CancellationToken cancellationToken)
        {
            

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.Id}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var owner = _mapper.Map<Owner>(command);
                if (uploadRequest != null)
                {
                    owner.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<Owner>().AddAsync(owner);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(owner.Id, _localizer["Owner Saved"]);
            }
            else
            {
                var owner = await _unitOfWork.Repository<Owner>().GetByIdAsync(command.Id);
                if (owner != null)
                {
                    owner.Name = command.Name ?? owner.Name;
                    owner.Description = command.Description ?? owner.Description;
                    if (uploadRequest != null)
                    {
                        owner.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
					
owner.PassportId = (command.PassportId == 0) ? owner.PassportId: command.PassportId;
                    //owner.Rate = (command.Rate == 0) ? owner.Rate : command.Rate;
                    //owner.BrandId = (command.BrandId == 0) ? owner.BrandId : command.BrandId;
                    await _unitOfWork.Repository<Owner>().UpdateAsync(owner);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(owner.Id, _localizer["Owner Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Owner Not Found!"]);
                }
            }
        }
    }
}