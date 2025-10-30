using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Events;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public EventCategoryService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<EventCategoryViewModel>> GetEventCategories()
        {
            var eventCategoriesEntities = await uow.Query<EventCategory>().ToListAsync();
            var eventCategoriesVM = mapper.Map<List<EventCategory>, List<EventCategoryViewModel>>(eventCategoriesEntities);
            return eventCategoriesVM;
        }

        public async Task<List<EventCategoryViewModel>> GetPagedEventCategories(string searchString, string orderBy)
        {
            var eventCategoriesEntities = await uow.Query<EventCategory>().ToListAsync();

            if (eventCategoriesEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    eventCategoriesEntities = eventCategoriesEntities.Where(x => x.Description.Contains(searchString) || x.Name.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        eventCategoriesEntities = eventCategoriesEntities.OrderBy(x => x.Name).ToList();
                    if (orderBy.Contains("Description"))
                        eventCategoriesEntities = eventCategoriesEntities.OrderBy(x => x.Description).ToList();
                    if (orderBy.Contains("RecordOrder"))
                        eventCategoriesEntities = eventCategoriesEntities.OrderBy(x => x.RecordOrder).ToList();
                }
            }

            var eventCategoriesVM = mapper.Map<List<EventCategory>, List<EventCategoryViewModel>>(eventCategoriesEntities);

            return eventCategoriesVM;

        }

        public async Task<EventCategoryViewModel> GetEventCategoryByID(int eventCategoryId)
        {
            var eventCategoriesEntity = await uow.Query<EventCategory>().Where(x => x.Id == eventCategoryId).FirstOrDefaultAsync();
            var eventCategoriesVM = mapper.Map<EventCategory, EventCategoryViewModel>(eventCategoriesEntity);
            
            return eventCategoriesVM;
        }

        public async Task<EventCategoryViewModel> AddEventCategory(EventCategoryInsertModel eventCategoryInsertModel)
        {
            try
            {
                var eventCategoryEntity = mapper.Map<EventCategoryInsertModel, EventCategory>(eventCategoryInsertModel);
                var result = uow.Add(eventCategoryEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<EventCategory, EventCategoryViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<EventCategoryViewModel> UpdateEventCategory(EventCategoryUpdateModel eventCategoryUpdateModel)
        {
            try
            {
                var eventCategoryEntity = uow.Query<EventCategory>().Where(x => x.Id == eventCategoryUpdateModel.Id).FirstOrDefault();
                if (eventCategoryEntity != null)
                {
                    eventCategoryEntity.Name = eventCategoryUpdateModel.Name;
                    eventCategoryEntity.Description = eventCategoryUpdateModel.Description;
                    eventCategoryEntity.EnglishName = eventCategoryUpdateModel.EnglishName;
                    eventCategoryEntity.EnglishDescription = eventCategoryUpdateModel.EnglishDescription;
                    eventCategoryEntity.Image = eventCategoryUpdateModel.Image;
                    eventCategoryEntity.RecordOrder = eventCategoryUpdateModel.RecordOrder;
                    eventCategoryEntity.IsActive = eventCategoryUpdateModel.IsActive;
                    uow.Update(eventCategoryEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<EventCategory, EventCategoryViewModel>(eventCategoryEntity);
                    return resultVM;
                }
                else
                    return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SoftDeleteEventCategory(int eventCategoryId) // enable/disable
        {
            try
            {
                var eventCategoryEntity = uow.Query<EventCategory>().Where(x => x.Id == eventCategoryId).FirstOrDefault();
                if (eventCategoryEntity != null)
                {
                    eventCategoryEntity.IsActive = !eventCategoryEntity.IsActive;
                    uow.Remove(eventCategoryEntity);
                    await SaveAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SaveAsync()
        {
            await uow.CommitAsync();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}

