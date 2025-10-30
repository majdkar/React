using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Blocks;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public class BlockPhotoService : IBlockPhotoService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public BlockPhotoService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<BlockPhotoViewModel>> GetPhotoByBlockId(int blockId)
        {
            var photoEntities = await uow.Query<BlockPhoto>().Where(x => x.BlockId == blockId).ToListAsync();
            var photosVM = mapper.Map<List<BlockPhoto>, List<BlockPhotoViewModel>>(photoEntities);
            return photosVM;
        }

        public async Task<BlockPhotoViewModel> GetPhotoById(int photoId)
        {
            var photoEntity = await uow.Query<BlockPhoto>().Where(x => x.Id == photoId).FirstOrDefaultAsync();
            var photoVM = mapper.Map<BlockPhoto, BlockPhotoViewModel>(photoEntity);
            return photoVM;
        }

        public async Task<BlockPhotoViewModel> AddPhoto(BlockPhotoInsertModel photoInsertModel)
        {
            try
            {
                var photoEntity = mapper.Map<BlockPhotoInsertModel, BlockPhoto>(photoInsertModel);
                var result = uow.Add(photoEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockPhoto, BlockPhotoViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlockPhotoViewModel> UpdatePhoto(BlockPhotoUpdateModel photoUpdateModel)
        {
            try
            {
                var photoEntity = uow.Query<BlockPhoto>().Where(x => x.Id == photoUpdateModel.Id).FirstOrDefault();
                if (photoEntity != null)
                {
                    photoEntity.Image = photoUpdateModel.Image;
                    photoEntity.BlockId = photoUpdateModel.BlockId;

                    uow.Update(photoEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockPhoto, BlockPhotoViewModel>(photoEntity);
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
                var photoEntity = uow.Query<BlockPhoto>().Where(x => x.Id == photoId).FirstOrDefault();
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
