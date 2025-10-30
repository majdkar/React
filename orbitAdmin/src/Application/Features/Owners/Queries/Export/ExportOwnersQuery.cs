using System;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Entities.OwnersManagement;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Specifications.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Features.Owners.Queries
{
    public class ExportOwnersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportOwnersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportOwnersQueryHandler : IRequestHandler<ExportOwnersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportOwnersQueryHandler> _localizer;

        public ExportOwnersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportOwnersQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportOwnersQuery request, CancellationToken cancellationToken)
        {
            var ownerFilterSpec = new OwnerFilterSpecification(request.SearchString);
            var owners = await _unitOfWork.Repository<Owner>().Entities
                .Specify(ownerFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(owners, mappers: new Dictionary<string, Func<Owner, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
				{ _localizer["Description"], item => item.Description },
				
                //{ _localizer["Barcode"], item => item.Barcode },
                //{ _localizer["Rate"], item => item.Rate }
            }, sheetName: _localizer["Owners"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}