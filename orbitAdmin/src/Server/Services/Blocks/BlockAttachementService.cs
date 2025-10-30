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
    public class BlockAttachementService : IBlockAttachementService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public BlockAttachementService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<BlockAttachementViewModel>> GetAttachementByBlockId(int blockId)
        {
            var AttachementEntities = await uow.Query<BlockAttachement>().Where(x => x.BlockId == blockId).ToListAsync();
            var AttachementsVM = mapper.Map<List<BlockAttachement>, List<BlockAttachementViewModel>>(AttachementEntities);
            return AttachementsVM;
        }

        public async Task<BlockAttachementViewModel> GetAttachementById(int AttachementId)
        {
            var AttachementEntity = await uow.Query<BlockAttachement>().Where(x => x.Id == AttachementId).FirstOrDefaultAsync();
            var AttachementVM = mapper.Map<BlockAttachement, BlockAttachementViewModel>(AttachementEntity);
            return AttachementVM;
        }

        public async Task<BlockAttachementViewModel> AddAttachement(BlockAttachementInsertModel AttachementInsertModel)
        {
            try
            {
                var AttachementEntity = mapper.Map<BlockAttachementInsertModel, BlockAttachement>(AttachementInsertModel);
                var result = uow.Add(AttachementEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockAttachement, BlockAttachementViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlockAttachementViewModel> UpdateAttachement(BlockAttachementUpdateModel AttachementUpdateModel)
        {
            try
            {
                var AttachementEntity = uow.Query<BlockAttachement>().Where(x => x.Id == AttachementUpdateModel.Id).FirstOrDefault();
                if (AttachementEntity != null)
                {
                    AttachementEntity.File = AttachementUpdateModel.File;
                    AttachementEntity.Name = AttachementUpdateModel.Name;
                    AttachementEntity.BlockId = AttachementUpdateModel.BlockId;

                    uow.Update(AttachementEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockAttachement, BlockAttachementViewModel>(AttachementEntity);
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
                var AttachementEntity = uow.Query<BlockAttachement>().Where(x => x.Id == AttachementId).FirstOrDefault();
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
