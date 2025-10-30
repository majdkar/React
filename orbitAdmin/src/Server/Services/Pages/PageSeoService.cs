using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Pages;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.Pages;

namespace SchoolV01.Application.Services
{
    public class PageSeoService : IPageSeoService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public PageSeoService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<PageSeoViewModel>> GetSeoByPageId(int PageId)
        {
            var SeoEntities = await uow.Query<PageSeo>().Where(x => x.PageId == PageId).ToListAsync();
            var SeosVM = mapper.Map<List<PageSeo>, List<PageSeoViewModel>>(SeoEntities);
            return SeosVM;
        }
        public async Task<PageSeoViewModel> GetSeoViewByPageId(int PageId)
        {
            var SeoEntity = await uow.Query<PageSeo>().Where(x => x.PageId == PageId).FirstOrDefaultAsync();
            var SeoVM = mapper.Map<PageSeo, PageSeoViewModel>(SeoEntity);
            return SeoVM;
        }

        public async Task<PageSeoViewModel> GetSeoById(int SeoId)
        {
            var SeoEntity = await uow.Query<PageSeo>().Where(x => x.Id == SeoId).FirstOrDefaultAsync();
            var SeoVM = mapper.Map<PageSeo, PageSeoViewModel>(SeoEntity);
            return SeoVM;
        }

        public async Task<PageSeoViewModel> AddSeo(PageSeoInsertModel SeoInsertModel)
        {
            try
            {
                var SeoEntity = mapper.Map<PageSeoInsertModel, PageSeo>(SeoInsertModel);
                var result = uow.Add(SeoEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<PageSeo, PageSeoViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PageSeoViewModel> UpdateSeo(PageSeoUpdateModel SeoUpdateModel)
        {
            try
            {
                var SeoEntity = uow.Query<PageSeo>().Where(x => x.Id == SeoUpdateModel.Id).FirstOrDefault();
                if (SeoEntity != null)
                {
                    SeoEntity.PageId = SeoUpdateModel.PageId;

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
                    var resultVM = mapper.Map<PageSeo, PageSeoViewModel>(SeoEntity);
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
                var SeoEntity = uow.Query<PageSeo>().Where(x => x.Id == SeoId).FirstOrDefault();
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
