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
    public class BlockCategoryService : IBlockCategoryService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public BlockCategoryService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<BlockCategoryViewModel>> GetBlockCategories()
        {
            var blockCategoriesEntities = await uow.Query<BlockCategory>().OrderByDescending(x => x.Id).ToListAsync();
            var blockCategoriesVM = mapper.Map<List<BlockCategory>, List<BlockCategoryViewModel>>(blockCategoriesEntities);
            return blockCategoriesVM;
        }

        public async Task<List<BlockCategoryViewModel>> GetPagedBlockCategories(string searchString, string orderBy)
        {
            var blockCategoriesEntities = await uow.Query<BlockCategory>().ToListAsync();

            if (blockCategoriesEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    blockCategoriesEntities = blockCategoriesEntities.Where(x => x.DescriptionAr.Contains(searchString) || x.NameAr.Contains(searchString) || x.BlockType.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        blockCategoriesEntities = [.. blockCategoriesEntities.OrderBy(x => x.NameAr)];
                    if (orderBy.Contains("Description"))
                        blockCategoriesEntities = [.. blockCategoriesEntities.OrderBy(x => x.DescriptionAr)];
                    if (orderBy.Contains("BlockType"))
                        blockCategoriesEntities = [.. blockCategoriesEntities.OrderBy(x => x.BlockType)];
                }
            }

            var blockCategoriesVM = mapper.Map<List<BlockCategory>, List<BlockCategoryViewModel>>(blockCategoriesEntities);

            return blockCategoriesVM;

        }

        public async Task<BlockCategoryViewModel> GetBlockCategoryByID(int blockCategoryId)
        {
            var blockCategoriesEntity = await uow.Query<BlockCategory>().Where(x => x.Id == blockCategoryId).FirstOrDefaultAsync();
            var blockCategoriesVM = mapper.Map<BlockCategory, BlockCategoryViewModel>(blockCategoriesEntity);
            return blockCategoriesVM;
        }

        public async Task<BlockCategoryViewModel> AddBlockCategory(BlockCategoryInsertModel blockCategoryInsertModel)
        {
            try
            {
                var blockCategoryEntity = mapper.Map<BlockCategoryInsertModel, BlockCategory>(blockCategoryInsertModel);
                var result = uow.Add(blockCategoryEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockCategory, BlockCategoryViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<BlockCategoryViewModel> UpdateBlockCategory(BlockCategoryUpdateModel blockCategoryUpdateModel)
        {
            try
            {
                var blockCategoryEntity = uow.Query<BlockCategory>().Where(x => x.Id == blockCategoryUpdateModel.Id).FirstOrDefault();
                if (blockCategoryEntity != null)
                {
                    blockCategoryEntity.NameAr = blockCategoryUpdateModel.NameAr;
                    blockCategoryEntity.DescriptionAr = blockCategoryUpdateModel.DescriptionAr;
                    blockCategoryEntity.NameEn = blockCategoryUpdateModel.NameEn;
                    blockCategoryEntity.DescriptionEn = blockCategoryUpdateModel.DescriptionEn;
                    blockCategoryEntity.NameGe = blockCategoryUpdateModel.NameGe;
                    blockCategoryEntity.DescriptionGe = blockCategoryUpdateModel.DescriptionGe;
                    blockCategoryEntity.BlockType = blockCategoryUpdateModel.BlockType;
                    blockCategoryEntity.IsActive = blockCategoryUpdateModel.IsActive;
                    uow.Update(blockCategoryEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockCategory, BlockCategoryViewModel>(blockCategoryEntity);
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

        public async Task<bool> SoftDeleteBlockCategory(int blockCategoryId) // enable/disable
        {
            try
            {
                var blockCategoryEntity = uow.Query<BlockCategory>().Where(x => x.Id == blockCategoryId).FirstOrDefault();
                if (blockCategoryEntity != null)
                {
                    blockCategoryEntity.IsActive = !blockCategoryEntity.IsActive;
                    uow.Remove(blockCategoryEntity);
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
