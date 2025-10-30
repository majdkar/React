using System;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.OwnersManagement;
using SchoolV01.Domain.Entities.OwnersManagement;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Owners.Queries
{
    public class GetAllOwnersQuery : IRequest<PaginatedResult<GetAllPagedOwnersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllOwnersQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllOwnersQueryHandler : IRequestHandler<GetAllOwnersQuery, PaginatedResult<GetAllPagedOwnersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllOwnersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedOwnersResponse>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Owner, GetAllPagedOwnersResponse>> expression = e => new GetAllPagedOwnersResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,

                PassportId = e.PassportId,
                //Rate = e.Rate,
                //Barcode = e.Barcode,
                //Brand = e.Brand.Name,
                //BrandId = e.BrandId
            };
            var ownerFilterSpec = new OwnerFilterSpecification(request.SearchString);
            if (request.OrderBy?.Length == null)
            {
                var data = await _unitOfWork.Repository<Owner>().Entities
                   .Specify(ownerFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Owner>().Entities
                   .Specify(ownerFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}