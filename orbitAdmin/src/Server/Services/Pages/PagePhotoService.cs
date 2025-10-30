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
    public class PagePhotoService : IPagePhotoService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public PagePhotoService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<PagePhotoViewModel>> GetPhotoByPageId(int pageId)
        {
            var photoEntities = await uow.Query<PagePhoto>().Where(x => x.PageId == pageId).ToListAsync();
            var photosVM = mapper.Map<List<PagePhoto>, List<PagePhotoViewModel>>(photoEntities);
            return photosVM;
        }

        public async Task<PagePhotoViewModel> GetPhotoById(int photoId)
        {
            var photoEntity = await uow.Query<PagePhoto>().Where(x => x.Id == photoId).FirstOrDefaultAsync();
            var photoVM = mapper.Map<PagePhoto, PagePhotoViewModel>(photoEntity);
            return photoVM;
        }

        public async Task<PagePhotoViewModel> AddPhoto(PagePhotoInsertModel photoInsertModel)
        {
            try
            {
                var photoEntity = mapper.Map<PagePhotoInsertModel, PagePhoto>(photoInsertModel);
                var result = uow.Add(photoEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<PagePhoto, PagePhotoViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PagePhotoViewModel> UpdatePhoto(PagePhotoUpdateModel photoUpdateModel)
        {
            try
            {
                var photoEntity = uow.Query<PagePhoto>().Where(x => x.Id == photoUpdateModel.Id).FirstOrDefault();
                if (photoEntity != null)
                {
                    photoEntity.Image = photoUpdateModel.Image;
                    photoEntity.PageId = photoUpdateModel.PageId;

                    uow.Update(photoEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<PagePhoto, PagePhotoViewModel>(photoEntity);
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
                var photoEntity = uow.Query<PagePhoto>().Where(x => x.Id == photoId).FirstOrDefault();
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
