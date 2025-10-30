using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Blocks;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Shared.ViewModels.Menus;

namespace SchoolV01.Application.Services
{
    public class BlockService : IBlockService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IBlockPhotoService photoService;
        private readonly IBlockSeoService seoService;
        private readonly IBlockAttachementService AttachementService;

        public BlockService(IUnitOfWork<int> uow, IMapper mapper,
            IBlockPhotoService photoService,
            IBlockSeoService seoService,
            IBlockAttachementService attachementService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.photoService = photoService;
            this.seoService = seoService;
            this.AttachementService = attachementService;
        }

        public async Task<List<BlockViewModel>> GetBlocks()
        {
            var blockEntities = await uow.Query<Block>().OrderBy(x => x.RecordOrder).ToListAsync();
            var blocksVM = mapper.Map<List<Block>, List<BlockViewModel>>(blockEntities);
            foreach (var item in blocksVM)
            {
                item.BlockPhotos = await photoService.GetPhotoByBlockId(item.Id);
                item.BlockAttachements = await AttachementService.GetAttachementByBlockId(item.Id);
            }
            return blocksVM;
        }
        
        public async Task<List<BlockEndpointViewModel>> GetEndpointBlocks(int categoryId)
        {
            var blockEntities = await uow.Query<Block>().Where(x => x.CategoryId == categoryId).OrderBy(x => x.RecordOrder).ToListAsync();
            var blocksVM = mapper.Map<List<Block>, List<BlockEndpointViewModel>>(blockEntities);
            return blocksVM;
        }

        public async Task<List<BlockViewModel>> GetPagedBlocks(string searchString, string orderBy)
        {
            var blockEntities = await uow.Query<Block>().OrderBy(x => x.RecordOrder).ToListAsync();

            if (blockEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    blockEntities = blockEntities.Where(x => x.DescriptionAr.Contains(searchString) || x.NameAr.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        blockEntities = [.. blockEntities.OrderBy(x => x.NameAr)];
                    if (orderBy.Contains("Description"))
                        blockEntities = [.. blockEntities.OrderBy(x => x.DescriptionAr)];
                    if (orderBy.Contains("Date"))
                        blockEntities = [.. blockEntities.OrderByDescending(x => x.Date)];
                }
            }

            var blocksVM = mapper.Map<List<Block>, List<BlockViewModel>>(blockEntities);
            foreach (var item in blocksVM)
            {
                item.BlockPhotos = await photoService.GetPhotoByBlockId(item.Id);
                item.BlockAttachements = await AttachementService.GetAttachementByBlockId(item.Id);
            }


            return blocksVM;
        }

        public async Task<List<BlockViewModel>> GetBlocksByCategoryId(int categoryId)
        {
            var blockEntities = await uow.Query<Block>().Where(x => x.CategoryId == categoryId && x.IsActive).OrderBy(x => x.RecordOrder).ToListAsync();
            var blocksVM = mapper.Map<List<Block>, List<BlockViewModel>>(blockEntities);
            foreach (var item in blocksVM)
            {
                item.BlockPhotos = await photoService.GetPhotoByBlockId(item.Id);
                item.BlockAttachements = await AttachementService.GetAttachementByBlockId(item.Id);
            }
            return blocksVM;
        }

        public async Task<BlockViewModel> GetBlockById(int blockId)
        {
            var blockEntity = await uow.Query<Block>().Where(x => x.Id == blockId).FirstOrDefaultAsync();
            var blockVM = mapper.Map<Block, BlockViewModel>(blockEntity);
            blockVM.BlockPhotos = await photoService.GetPhotoByBlockId(blockVM.Id);
            blockVM.BlockAttachements = await AttachementService.GetAttachementByBlockId(blockVM.Id);
            blockVM.BlockSeo = await seoService.GetSeoViewByBlockId(blockVM.Id);
            return blockVM;
        }


        public async Task<BlockViewModel> GetBlockByEndpoint(string Endpoint)
        {
            var blockEntity = await uow.Query<Block>().Where(x => x.EndpointAr == Endpoint || x.EndpointEn == Endpoint || x.EndpointGe == Endpoint).FirstOrDefaultAsync();
            var blockVM = mapper.Map<Block, BlockViewModel>(blockEntity);
            blockVM.BlockPhotos = await photoService.GetPhotoByBlockId(blockVM.Id);
            blockVM.BlockAttachements = await AttachementService.GetAttachementByBlockId(blockVM.Id);
            blockVM.BlockSeo = await seoService.GetSeoViewByBlockId(blockVM.Id);

            return blockVM;
        }

        public async Task<BlockViewModel> AddBlock(BlockInsertModel blockInsertModel)
        {
            try
            {
                var blockEntity = mapper.Map<BlockInsertModel, Block>(blockInsertModel);
                var result = uow.Add(blockEntity);
                await SaveAsync();
                if (result != null)
                {
                    blockEntity.Url = $"/{blockEntity.Id}";
                    uow.Update(blockEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Block, BlockViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlockViewModel> UpdateBlock(BlockUpdateModel blockUpdateModel)
        {
            try
            {
                var blockEntity = uow.Query<Block>().Where(x => x.Id == blockUpdateModel.Id).FirstOrDefault();
                if (blockEntity != null)
                {
                    blockEntity.NameAr = blockUpdateModel.NameAr?? blockEntity.NameAr;
                    blockEntity.DescriptionAr = blockUpdateModel.DescriptionAr ?? blockEntity.DescriptionAr;
                    blockEntity.DescriptionAr1 = blockUpdateModel.DescriptionAr1 ?? blockEntity.DescriptionAr1;
                    blockEntity.DescriptionAr2 = blockUpdateModel.DescriptionAr2 ?? blockEntity.DescriptionAr2;
                    blockEntity.DescriptionAr3 = blockUpdateModel.DescriptionAr3 ?? blockEntity.DescriptionAr3;
                    blockEntity.NameEn = blockUpdateModel.NameEn ?? blockEntity.NameEn;
                    blockEntity.DescriptionEn = blockUpdateModel.DescriptionEn ?? blockEntity.DescriptionEn;
                    blockEntity.DescriptionEn1 = blockUpdateModel.DescriptionEn1 ?? blockEntity.DescriptionEn1;
                    blockEntity.DescriptionEn2 = blockUpdateModel.DescriptionEn2 ?? blockEntity.DescriptionEn2;
                    blockEntity.DescriptionEn3 = blockUpdateModel.DescriptionEn3 ?? blockEntity.DescriptionEn3;
                    blockEntity.NameGe = blockUpdateModel.NameGe ?? blockEntity.NameGe;
                    blockEntity.DescriptionGe = blockUpdateModel.DescriptionGe ?? blockEntity.DescriptionGe;
                    blockEntity.DescriptionGe1 = blockUpdateModel.DescriptionGe1 ?? blockEntity.DescriptionGe1;
                    blockEntity.DescriptionGe2 = blockUpdateModel.DescriptionGe2 ?? blockEntity.DescriptionGe2;
                    blockEntity.DescriptionGe3 = blockUpdateModel.DescriptionGe3 ?? blockEntity.DescriptionGe3;
                    blockEntity.SeoDescription = blockUpdateModel.SeoDescription ?? blockEntity.SeoDescription;
                    blockEntity.EndpointAr = blockUpdateModel.EndpointAr ?? blockEntity.EndpointAr;
                    blockEntity.EndpointEn = blockUpdateModel.EndpointEn ?? blockEntity.EndpointEn;
                    blockEntity.EndpointGe = blockUpdateModel.EndpointGe ?? blockEntity.EndpointGe;
                    blockEntity.Keywords = blockUpdateModel.Keywords ?? blockEntity.Keywords;
                    blockEntity.Date = blockUpdateModel.Date;
                    blockEntity.StartDate = blockUpdateModel.StartDate;
                    blockEntity.EndDate = blockUpdateModel.EndDate;
                    blockEntity.CreateAt = blockUpdateModel.CreateAt;
                    blockEntity.Location = blockUpdateModel.Location;
                    blockEntity.AuthorId = blockUpdateModel.AuthorId;
                    blockEntity.RecordOrder = blockUpdateModel.RecordOrder;
                    blockEntity.Image = blockUpdateModel.Image?? blockEntity.Image;
                    blockEntity.File = blockUpdateModel.File?? blockEntity.File;
                    blockEntity.Url = blockUpdateModel.Url?? blockEntity.Url;
                    blockEntity.Url1 = blockUpdateModel.Url1?? blockEntity.Url1;
                    blockEntity.IsVisible = blockUpdateModel.IsVisible;
                    blockEntity.IsActive = blockUpdateModel.IsActive;
                    blockEntity.ParentId = blockUpdateModel.ParentId;

                    blockEntity.Image1 = blockUpdateModel.Image1 ?? blockEntity.Image1;
                    blockEntity.Image2 = blockUpdateModel.Image2 ?? blockEntity.Image2;
                    blockEntity.Image3 = blockUpdateModel.Image3 ?? blockEntity.Image3;
                    
                    //blockToUpdate.CategoryId = blockUpdateModel.CategoryId; // category is not changable
                    if (string.IsNullOrEmpty(blockEntity.Url))
                    {
                        blockEntity.Url = $"/{blockEntity.Id}";
                    }
                    uow.Update(blockEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Block, BlockViewModel>(blockEntity);
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

        public async Task<bool> SoftDeleteBlock(int blockId)
        {
            try
            {
                var blockEntity = uow.Query<Block>().Where(x => x.Id == blockId).FirstOrDefault();
                if (blockEntity != null)
                {
                    blockEntity.IsActive = !blockEntity.IsActive;
                    uow.Remove(blockEntity);
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
