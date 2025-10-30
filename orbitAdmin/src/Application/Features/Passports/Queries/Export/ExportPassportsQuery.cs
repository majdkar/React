using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Specifications.OwnersManagement;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Features.Passports.Queries
{
    public class ExportPassportsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPassportsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPassportsQueryHandler : IRequestHandler<ExportPassportsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportPassportsQueryHandler> _localizer;

        public ExportPassportsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportPassportsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPassportsQuery request, CancellationToken cancellationToken)
        {
            var passportFilterSpec = new PassportFilterSpecification(request.SearchString);
            var passports = await _unitOfWork.Repository<Passport>().Entities
                .Specify(passportFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(passports, mappers: new Dictionary<string, Func<Passport, object>>
            {
				{ _localizer["Id"], item => item.Id },
				

                //{ _localizer["Tax"], item => item.Tax }
            }, sheetName: _localizer["Passports"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
