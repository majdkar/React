using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Blocks;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.Blocks;
using SchoolV01.Domain.Entities.Pages;
using SchoolV01.Shared.ViewModels.Pages;

namespace SchoolV01.Application.Services
{
    public class BlockSeoService : IBlockSeoService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public BlockSeoService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<BlockSeoViewModel>> GetSeoByBlockId(int blockId)
        {
            var SeoEntities = await uow.Query<BlockSeo>().Where(x => x.BlockId == blockId).ToListAsync();
            var SeosVM = mapper.Map<List<BlockSeo>, List<BlockSeoViewModel>>(SeoEntities);
            return SeosVM;
        }


        public async Task<BlockSeoViewModel> GetSeoViewByBlockId(int BlockId)
        {
            var SeoEntity = await uow.Query<BlockSeo>().Where(x => x.BlockId == BlockId).FirstOrDefaultAsync();
            var SeoVM = mapper.Map<BlockSeo, BlockSeoViewModel>(SeoEntity);
            return SeoVM;
        }


        public async Task<BlockSeoViewModel> GetSeoById(int SeoId)
        {
            var SeoEntity = await uow.Query<BlockSeo>().Where(x => x.Id == SeoId).FirstOrDefaultAsync();
            var SeoVM = mapper.Map<BlockSeo, BlockSeoViewModel>(SeoEntity);
            return SeoVM;
        }

        public async Task<BlockSeoViewModel> AddSeo(BlockSeoInsertModel SeoInsertModel)
        {
            try
            {
                var SeoEntity = mapper.Map<BlockSeoInsertModel, BlockSeo>(SeoInsertModel);
                var result = uow.Add(SeoEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockSeo, BlockSeoViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlockSeoViewModel> UpdateSeo(BlockSeoUpdateModel SeoUpdateModel)
        {
            try
            {
                var SeoEntity = uow.Query<BlockSeo>().Where(x => x.Id == SeoUpdateModel.Id).FirstOrDefault();
                if (SeoEntity != null)
                {
                    SeoEntity.BlockId = SeoUpdateModel.BlockId;

                    SeoEntity.MetaTitleAr = SeoUpdateModel.MetaTitleAr;
                    SeoEntity.MetaTitleEn = SeoUpdateModel.MetaTitleEn;
                    SeoEntity.MetaTitleGe = SeoUpdateModel.MetaTitleGe;

                    SeoEntity.MetaNameAr = SeoUpdateModel.MetaNameAr;
                    SeoEntity.MetaNameEn = SeoUpdateModel.MetaNameEn;
                    SeoEntity.MetaNameGe = SeoUpdateModel.MetaNameGe;

                    SeoEntity.MetaRobots = SeoUpdateModel.MetaRobots;


                    SeoEntity.MetaUrlAr = SeoUpdateModel.MetaUrlAr;
                    SeoEntity.MetaUrlEn = SeoUpdateModel.MetaUrlEn;
                    SeoEntity.MetaUrlGe = SeoUpdateModel.MetaUrlGe;

                    SeoEntity.MetaKeywordsAr = SeoUpdateModel.MetaKeywordsAr;
                    SeoEntity.MetaKeywordsEn = SeoUpdateModel.MetaKeywordsEn;
                    SeoEntity.MetaKeywordsGe = SeoUpdateModel.MetaKeywordsGe;

                    SeoEntity.MetaDescriptionsAr = SeoUpdateModel.MetaDescriptionsAr;
                    SeoEntity.MetaDescriptionsEn = SeoUpdateModel.MetaDescriptionsEn;
                    SeoEntity.MetaDescriptionsGe = SeoUpdateModel.MetaDescriptionsGe;


                    SeoEntity.ImageAlt1Ar = SeoUpdateModel.ImageAlt1Ar;
                    SeoEntity.ImageAlt1En = SeoUpdateModel.ImageAlt1En;
                    SeoEntity.ImageAlt1Ge = SeoUpdateModel.ImageAlt1Ge;

                    SeoEntity.ImageAlt2Ar = SeoUpdateModel.ImageAlt2Ar;
                    SeoEntity.ImageAlt2En = SeoUpdateModel.ImageAlt2En;
                    SeoEntity.ImageAlt2Ge = SeoUpdateModel.ImageAlt2Ge;

                    SeoEntity.ImageAlt3Ar = SeoUpdateModel.ImageAlt3Ar;
                    SeoEntity.ImageAlt3En = SeoUpdateModel.ImageAlt3En;
                    SeoEntity.ImageAlt3Ge = SeoUpdateModel.ImageAlt3Ge;

                    SeoEntity.ImageAlt4Ar = SeoUpdateModel.ImageAlt4Ar;
                    SeoEntity.ImageAlt4En = SeoUpdateModel.ImageAlt4En;
                    SeoEntity.ImageAlt4Ge = SeoUpdateModel.ImageAlt4Ge;



                     
                    uow.Update(SeoEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockSeo, BlockSeoViewModel>(SeoEntity);
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
        
        public async Task<bool> SoftDeleteSeo(int SeoId)
        {
            try
            {
                var SeoEntity = uow.Query<BlockSeo>().Where(x => x.Id == SeoId).FirstOrDefault();
                if (SeoEntity != null)
                {
                    uow.Remove(SeoEntity);
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
