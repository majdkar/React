using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Pages;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public class PageAttachementService : IPageAttachementService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public PageAttachementService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<PageAttachementViewModel>> GetAttachementByPageId(int pageId)
        {
            var AttachementEntities = await uow.Query<PageAttachement>().Where(x => x.PageId == pageId).ToListAsync();
            var AttachementsVM = mapper.Map<List<PageAttachement>, List<PageAttachementViewModel>>(AttachementEntities);
            return AttachementsVM;
        }

        public async Task<PageAttachementViewModel> GetAttachementById(int AttachementId)
        {
            var AttachementEntity = await uow.Query<PageAttachement>().Where(x => x.Id == AttachementId).FirstOrDefaultAsync();
            var AttachementVM = mapper.Map<PageAttachement, PageAttachementViewModel>(AttachementEntity);
            return AttachementVM;
        }

        public async Task<PageAttachementViewModel> AddAttachement(PageAttachementInsertModel AttachementInsertModel)
        {
            try
            {
                var AttachementEntity = mapper.Map<PageAttachementInsertModel, PageAttachement>(AttachementInsertModel);
                var result = uow.Add(AttachementEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<PageAttachement, PageAttachementViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PageAttachementViewModel> UpdateAttachement(PageAttachementUpdateModel AttachementUpdateModel)
        {
            try
            {
                var AttachementEntity = uow.Query<PageAttachement>().Where(x => x.Id == AttachementUpdateModel.Id).FirstOrDefault();
                if (AttachementEntity != null)
                {
                    AttachementEntity.File = AttachementUpdateModel.File;
                    AttachementEntity.Name = AttachementUpdateModel.Name;
                    AttachementEntity.PageId = AttachementUpdateModel.PageId;

                    uow.Update(AttachementEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<PageAttachement, PageAttachementViewModel>(AttachementEntity);
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
                var AttachementEntity = uow.Query<PageAttachement>().Where(x => x.Id == AttachementId).FirstOrDefault();
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
