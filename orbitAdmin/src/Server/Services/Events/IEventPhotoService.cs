using SchoolV01.Shared.ViewModels.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IEventPhotoService
    {
        Task<List<EventPhotoViewModel>> GetPhotoByEventId(int eventId);

        Task<EventPhotoViewModel> GetPhotoById(int translationId);

        Task<EventPhotoViewModel> AddPhoto(EventPhotoInsertModel translationInsertModel);

        Task<EventPhotoViewModel> UpdatePhoto(EventPhotoUpdateModel translationUpdateModel);

        Task<bool> SoftDeletePhoto(int translationId);

        Task SaveAsync();
    }
}
