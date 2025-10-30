using SchoolV01.Shared.ViewModels.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IEventAttachementService
    {
        Task<List<EventAttachementViewModel>> GetAttachementByEventId(int eventId);

        Task<EventAttachementViewModel> GetAttachementById(int translationId);

        Task<EventAttachementViewModel> AddAttachement(EventAttachementInsertModel translationInsertModel);

        Task<EventAttachementViewModel> UpdateAttachement(EventAttachementUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteAttachement(int translationId);

        Task SaveAsync();
    }
}
