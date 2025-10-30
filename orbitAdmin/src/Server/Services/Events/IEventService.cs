using SchoolV01.Shared.ViewModels.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IEventService
    {
        Task<List<EventViewModel>> GetEvents();

        Task<List<EventViewModel>> GetPagedEvents(string searchString, string orderBy);

        Task<List<EventViewModel>> GetEventsByCategoryId(int categoryId);

        Task<EventViewModel> GetEventById(int eventId);

        Task<EventViewModel> AddEvent(EventInsertModel eventInsertModel);

        Task<EventViewModel> UpdateEvent(EventUpdateModel eventUpdateModel);

        Task<bool> SoftDeleteEvent(int eventId);

        Task SaveAsync();
    }
}
