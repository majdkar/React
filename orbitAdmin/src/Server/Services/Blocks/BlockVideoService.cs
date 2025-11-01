using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities;
using SchoolV01.Shared.ViewModels.Blocks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Server.Services.Blocks
{
    public class BlockVideoService : IBlockVideoService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public BlockVideoService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<BlockVideoViewModel>> GetVideoByBlockId(int blockId)
        {
            var VideoEntities = await uow.Query<BlockVideo>().Where(x => x.BlockId == blockId).ToListAsync();
            var VideosVM = mapper.Map<List<BlockVideo>, List<BlockVideoViewModel>>(VideoEntities);
            return VideosVM;
        }

        public async Task<BlockVideoViewModel> GetVideoById(int VideoId)
        {
            var VideoEntity = await uow.Query<BlockVideo>().Where(x => x.Id == VideoId).FirstOrDefaultAsync();
            var VideoVM = mapper.Map<BlockVideo, BlockVideoViewModel>(VideoEntity);
            return VideoVM;
        }

        public async Task<BlockVideoViewModel> AddVideo(BlockVideoInsertModel VideoInsertModel)
        {
            try
            {
                var VideoEntity = mapper.Map<BlockVideoInsertModel, BlockVideo>(VideoInsertModel);
                var result = uow.Add(VideoEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockVideo, BlockVideoViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<BlockVideoViewModel> UpdateVideo(BlockVideoUpdateModel VideoUpdateModel)
        {
            try
            {
                var VideoEntity = uow.Query<BlockVideo>().Where(x => x.Id == VideoUpdateModel.Id).FirstOrDefault();
                if (VideoEntity != null)
                {
                    VideoEntity.Url = VideoUpdateModel.Url;
                    VideoEntity.BlockId = VideoUpdateModel.BlockId;

                    uow.Update(VideoEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockVideo, BlockVideoViewModel>(VideoEntity);
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

        public async Task<bool> SoftDeleteVideo(int VideoId)
        {
            try
            {
                var VideoEntity = uow.Query<BlockVideo>().Where(x => x.Id == VideoId).FirstOrDefault();
                if (VideoEntity != null)
                {
                    uow.Remove(VideoEntity);
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
