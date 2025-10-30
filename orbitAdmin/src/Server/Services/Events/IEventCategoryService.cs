using SchoolV01.Shared.ViewModels.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IEventCategoryService
    {
        Task<List<EventCategoryViewModel>> GetEventCategories();

        Task<List<EventCategoryViewModel>> GetPagedEventCategories(string searchString, string orderBy);

        Task<EventCategoryViewModel> GetEventCategoryByID(int eventCategoryId);

        Task<EventCategoryViewModel> AddEventCategory(EventCategoryInsertModel eventCategoryInsertModel);

        Task<EventCategoryViewModel> UpdateEventCategory(EventCategoryUpdateModel eventCategoryUpdateModel);

        Task<bool> SoftDeleteEventCategory(int EventCategoryId);

        Task SaveAsync();
    }
}
