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
    public class EventPhotoService : IEventPhotoService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public EventPhotoService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<EventPhotoViewModel>> GetPhotoByEventId(int eventId)
        {
            var photoEntities = await uow.Query<EventPhoto>().Where(x => x.EventId == eventId).ToListAsync();
            var photosVM = mapper.Map<List<EventPhoto>, List<EventPhotoViewModel>>(photoEntities);
            return photosVM;
        }

        public async Task<EventPhotoViewModel> GetPhotoById(int photoId)
        {
            var photoEntity = await uow.Query<EventPhoto>().Where(x => x.Id == photoId).FirstOrDefaultAsync();
            var photoVM = mapper.Map<EventPhoto, EventPhotoViewModel>(photoEntity);
            return photoVM;
        }

        public async Task<EventPhotoViewModel> AddPhoto(EventPhotoInsertModel photoInsertModel)
        {
            try
            {
                var photoEntity = mapper.Map<EventPhotoInsertModel, EventPhoto>(photoInsertModel);
                var result = uow.Add(photoEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<EventPhoto, EventPhotoViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EventPhotoViewModel> UpdatePhoto(EventPhotoUpdateModel photoUpdateModel)
        {
            try
            {
                var photoEntity = uow.Query<EventPhoto>().Where(x => x.Id == photoUpdateModel.Id).FirstOrDefault();
                if (photoEntity != null)
                {
                    photoEntity.Image = photoUpdateModel.Image;
                    photoEntity.EventId = photoUpdateModel.EventId;

                    uow.Update(photoEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<EventPhoto, EventPhotoViewModel>(photoEntity);
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
        
        public async Task<bool> SoftDeletePhoto(int photoId)
        {
            try
            {
                var photoEntity = uow.Query<EventPhoto>().Where(x => x.Id == photoId).FirstOrDefault();
                if (photoEntity != null)
                {
                    uow.Remove(photoEntity);
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
