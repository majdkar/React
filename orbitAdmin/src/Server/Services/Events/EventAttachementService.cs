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
    public class EventAttachementService : IEventAttachementService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public EventAttachementService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<EventAttachementViewModel>> GetAttachementByEventId(int eventId)
        {
            var AttachementEntities = await uow.Query<EventAttachement>().Where(x => x.EventId == eventId).ToListAsync();
            var AttachementsVM = mapper.Map<List<EventAttachement>, List<EventAttachementViewModel>>(AttachementEntities);
            return AttachementsVM;
        }

        public async Task<EventAttachementViewModel> GetAttachementById(int AttachementId)
        {
            var AttachementEntity = await uow.Query<EventAttachement>().Where(x => x.Id == AttachementId).FirstOrDefaultAsync();
            var AttachementVM = mapper.Map<EventAttachement, EventAttachementViewModel>(AttachementEntity);
            return AttachementVM;
        }

        public async Task<EventAttachementViewModel> AddAttachement(EventAttachementInsertModel AttachementInsertModel)
        {
            try
            {
                var AttachementEntity = mapper.Map<EventAttachementInsertModel, EventAttachement>(AttachementInsertModel);
                var result = uow.Add(AttachementEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<EventAttachement, EventAttachementViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EventAttachementViewModel> UpdateAttachement(EventAttachementUpdateModel AttachementUpdateModel)
        {
            try
            {
                var AttachementEntity = uow.Query<EventAttachement>().Where(x => x.Id == AttachementUpdateModel.Id).FirstOrDefault();
                if (AttachementEntity != null)
                {
                    AttachementEntity.File = AttachementUpdateModel.File;
                    AttachementEntity.Name = AttachementUpdateModel.Name;
                    AttachementEntity.EventId = AttachementUpdateModel.EventId;

                    uow.Update(AttachementEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<EventAttachement, EventAttachementViewModel>(AttachementEntity);
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
        
        public async Task<bool> SoftDeleteAttachement(int AttachementId)
        {
            try
            {
                var AttachementEntity = uow.Query<EventAttachement>().Where(x => x.Id == AttachementId).FirstOrDefault();
                if (AttachementEntity != null)
                {
                    uow.Remove(AttachementEntity);
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
