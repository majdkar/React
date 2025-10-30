using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Extensions;

namespace SchoolV01.Application.Features.Cities.Queries
{
    public class ExportCitiesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCitiesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCitiesQueryHandler : IRequestHandler<ExportCitiesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCitiesQueryHandler> _localizer;

        public ExportCitiesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCitiesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCitiesQuery request, CancellationToken cancellationToken)
        {
            var Cities = await _unitOfWork.Repository<City>().Entities
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Cities, mappers: new Dictionary<string, Func<City, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["ArabicName"], item => item.NameAr },
               

                { _localizer["EnglishName"],item => item.NameEn },
               
            }, sheetName: _localizer["Cities"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }

}
