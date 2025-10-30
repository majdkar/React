using SchoolV01.Shared.ViewModels.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IPageAttachementService
    {
        Task<List<PageAttachementViewModel>> GetAttachementByPageId(int pageId);

        Task<PageAttachementViewModel> GetAttachementById(int translationId);

        Task<PageAttachementViewModel> AddAttachement(PageAttachementInsertModel translationInsertModel);

        Task<PageAttachementViewModel> UpdateAttachement(PageAttachementUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteAttachement(int translationId);

        Task SaveAsync();
    }
}
