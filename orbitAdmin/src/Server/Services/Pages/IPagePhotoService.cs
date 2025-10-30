using SchoolV01.Shared.ViewModels.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IPagePhotoService
    {
        Task<List<PagePhotoViewModel>> GetPhotoByPageId(int pageId);

        Task<PagePhotoViewModel> GetPhotoById(int translationId);

        Task<PagePhotoViewModel> AddPhoto(PagePhotoInsertModel translationInsertModel);

        Task<PagePhotoViewModel> UpdatePhoto(PagePhotoUpdateModel translationUpdateModel);

        Task<bool> SoftDeletePhoto(int translationId);

        Task SaveAsync();
    }
}
