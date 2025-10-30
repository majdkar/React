using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Misc;

namespace SchoolV01.Infrastructure.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly IRepositoryAsync<DocumentType, int> _repository;

        public DocumentTypeRepository(IRepositoryAsync<DocumentType, int> repository)
        {
            _repository = repository;
        }
    }
}