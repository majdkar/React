using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.Countries.Queries
{
    public class ExportCountriesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCountriesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCountriesQueryHandler : IRequestHandler<ExportCountriesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCountriesQueryHandler> _localizer;

        public ExportCountriesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCountriesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCountriesQuery request, CancellationToken cancellationToken)
        {
            var Countries = await _unitOfWork.Repository<Country>().Entities
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Countries, mappers: new Dictionary<string, Func<Country, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["ArabicName"], item => item.NameAr },
               

                { _localizer["EnglishName"],item => item.NameEn },
               
            }, sheetName: _localizer["Countries"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }

}
