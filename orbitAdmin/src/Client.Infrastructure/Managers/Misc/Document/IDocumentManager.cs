using SchoolV01.Application.Features.Documents.Commands;
using SchoolV01.Application.Features.Documents.Queries;
using SchoolV01.Application.Requests.Documents;
using SchoolV01.Shared.Wrapper;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}