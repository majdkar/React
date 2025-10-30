using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.DocumentTypes.Commands;
using SchoolV01.Application.Features.DocumentTypes.Queries;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Client.Infrastructure.Managers.Misc.DocumentType
{
    public interface IDocumentTypeManager : IManager
    {
        Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}