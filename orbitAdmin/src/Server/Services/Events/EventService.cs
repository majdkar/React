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
    public class EventService : IEventService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IEventPhotoService photoService;
        private readonly IEventAttachementService AttachementService;

        public EventService(IUnitOfWork<int> uow, IMapper mapper,
            IEventPhotoService photoService,
            IEventAttachementService attachementService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.photoService = photoService;
            this.AttachementService = attachementService;
        }

        public async Task<List<EventViewModel>> GetEvents()
        {
            var eventEntities = await uow.Query<Event>().ToListAsync();
            var eventsVM = mapper.Map<List<Event>, List<EventViewModel>>(eventEntities);
            foreach (var item in eventsVM)
            {
                item.EventPhotos = await photoService.GetPhotoByEventId(item.Id);
                item.EventAttachements = await AttachementService.GetAttachementByEventId(item.Id);
            }
            return eventsVM;
        }

        public async Task<List<EventViewModel>> GetPagedEvents(string searchString, string orderBy)
        {
            var eventEntities = await uow.Query<Event>().ToListAsync();

            if (eventEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    eventEntities = eventEntities.Where(x => x.Name.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        eventEntities = eventEntities.OrderBy(x => x.Name).ToList();
                    if (orderBy.Contains("RecordOrder"))
                        eventEntities = eventEntities.OrderBy(x => x.RecordOrder).ToList();
                }
            }

            var eventsVM = mapper.Map<List<Event>, List<EventViewModel>>(eventEntities);

            foreach (var item in eventsVM)
            {
                item.EventPhotos = await photoService.GetPhotoByEventId(item.Id);
                item.EventAttachements = await AttachementService.GetAttachementByEventId(item.Id);
            }

            return eventsVM;
        }

        public async Task<List<EventViewModel>> GetEventsByCategoryId(int categoryId)
        {
            var eventEntities = await uow.Query<Event>().Where(x => x.CategoryId == categoryId).ToListAsync();
            var eventsVM = mapper.Map<List<Event>, List<EventViewModel>>(eventEntities);

            foreach (var item in eventsVM)
            {
                item.EventPhotos = await photoService.GetPhotoByEventId(item.Id);
                item.EventAttachements = await AttachementService.GetAttachementByEventId(item.Id);
            }
            return eventsVM;
        }

        public async Task<EventViewModel> GetEventById(int eventId)
        {
            var eventEntity = await uow.Query<Event>().Where(x => x.Id == eventId).FirstOrDefaultAsync();
            var eventVM = mapper.Map<Event, EventViewModel>(eventEntity);
            eventVM.EventPhotos = await photoService.GetPhotoByEventId(eventVM.Id);
            eventVM.EventAttachements = await AttachementService.GetAttachementByEventId(eventVM.Id);
            return eventVM;
        }

        public async Task<EventViewModel> AddEvent(EventInsertModel eventInsertModel)
        {
            try
            {
                var eventEntity = mapper.Map<EventInsertModel, Event>(eventInsertModel);
                var result = uow.Add(eventEntity);
                await SaveAsync();
                if (result != null)
                {
                    eventEntity.Url = $"/{eventEntity.Id}";
                    uow.Update(eventEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Event, EventViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EventViewModel> UpdateEvent(EventUpdateModel eventUpdateModel)
        {
            try
            {
                var eventEntity = uow.Query<Event>().Where(x => x.Id == eventUpdateModel.Id).FirstOrDefault();
                if (eventEntity != null)
                {
                    eventEntity.Name = eventUpdateModel.Name;
                    eventEntity.Description = eventUpdateModel.Description;
                    eventEntity.EnglishName = eventUpdateModel.EnglishName;
                    eventEntity.EnglishDescription = eventUpdateModel.EnglishDescription;
                    eventEntity.StartDate = eventUpdateModel.StartDate;
                    eventEntity.EndDate = eventUpdateModel.EndDate;
                    eventEntity.RecordOrder = eventUpdateModel.RecordOrder;
                    eventEntity.Image = eventUpdateModel.Image;
                    eventEntity.Video = eventUpdateModel.Video;
                    eventEntity.File = eventUpdateModel.File;
                    eventEntity.Description = eventUpdateModel.Description;
                    eventEntity.IsVisible = eventUpdateModel.IsVisible;
                    eventEntity.IsActive = eventUpdateModel.IsActive;
                    eventEntity.Url = string.IsNullOrEmpty(eventEntity.Url) ? $"/{eventEntity.Id}" : eventUpdateModel.Url;
                    //eventEntity.CategoryId = eventUpdateModel.CategoryId; // category is not changable
                    uow.Update(eventEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Event, EventViewModel>(eventEntity);
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

        public async Task<bool> SoftDeleteEvent(int eventId)
        {
            try
            {
                var eventEntity = uow.Query<Event>().Where(x => x.Id == eventId).FirstOrDefault();
                if (eventEntity != null)
                {
                    eventEntity.IsActive = !eventEntity.IsActive;
                    uow.Remove(eventEntity);
                    await SaveAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception )
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
