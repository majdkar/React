using SchoolV01.Shared.ViewModels.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IPageSeoService
    {
        Task<List<PageSeoViewModel>> GetSeoByPageId(int PageId);
        Task<PageSeoViewModel> GetSeoViewByPageId(int PageId);

        Task<PageSeoViewModel> GetSeoById(int translationId);

        Task<PageSeoViewModel> AddSeo(PageSeoInsertModel translationInsertModel);

        Task<PageSeoViewModel> UpdateSeo(PageSeoUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteSeo(int translationId);

        Task SaveAsync();
    }
}
