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
    public class PageService : IPageService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IPagePhotoService photoService;
        private readonly IPageSeoService seoService;
        private readonly IPageAttachementService AttachementService;

        public PageService(IUnitOfWork<int> uow, IMapper mapper,
            IPagePhotoService photoService,
            IPageSeoService seoService,
            IPageAttachementService AttachementService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.photoService = photoService;
            this.seoService = seoService;
            this.AttachementService = AttachementService;
        }

        public async Task<List<PageViewModel>> GetPages()
        {
            var pagesEntities = await uow.Query<Page>().ToListAsync();
            var PagesVM = mapper.Map<List<Page>, List<PageViewModel>>(pagesEntities);

            foreach (var item in PagesVM)
            {
                item.PagePhotos = await photoService.GetPhotoByPageId(item.Id);
                item.PageAttachements = await AttachementService.GetAttachementByPageId(item.Id);
            }
            return PagesVM;
        }

        public async Task<List<PageViewModel>> GetPagedPages(string searchString, string orderBy)
        {
            var pagesEntities = await uow.Query<Page>().ToListAsync();

            if (pagesEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    pagesEntities = pagesEntities.Where(x => x.NameAr.Contains(searchString) || x.Type.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        pagesEntities = pagesEntities.OrderBy(x => x.NameAr).ToList();
                    if (orderBy.Contains("Type"))
                        pagesEntities = pagesEntities.OrderBy(x => x.Type).ToList();
                }
            }

            var pagesVM = mapper.Map<List<Page>, List<PageViewModel>>(pagesEntities);
            foreach (var item in pagesVM)
            {
                item.PagePhotos = await photoService.GetPhotoByPageId(item.Id);
                item.PageAttachements = await AttachementService.GetAttachementByPageId(item.Id);
            }

            return pagesVM;

        }

        public async Task<PageViewModel> GetPageByID(int pageId)
        {
            var pagesEntity = await uow.Query<Page>().Where(x => x.Id == pageId).FirstOrDefaultAsync();
            var pageVM = mapper.Map<Page, PageViewModel>(pagesEntity);
            pageVM.PagePhotos = await photoService.GetPhotoByPageId(pageVM.Id);
            pageVM.PageAttachements = await AttachementService.GetAttachementByPageId(pageVM.Id);
            pageVM.PageSeo = await seoService.GetSeoViewByPageId(pageVM.Id);
            return pageVM;
        }
        public async Task<PageViewModel> GetPageByEndpoint(string Endpoint)
        {
            var pagesEntity = await uow.Query<Page>().Where(x => x.EndpointAr == Endpoint || x.EndpointEn == Endpoint || x.EndpointGe == Endpoint).FirstOrDefaultAsync();
            var pageVM = mapper.Map<Page, PageViewModel>(pagesEntity);
            pageVM.PagePhotos = await photoService.GetPhotoByPageId(pageVM.Id);
            pageVM.PageAttachements = await AttachementService.GetAttachementByPageId(pageVM.Id);
            pageVM.PageSeo = await seoService.GetSeoViewByPageId(pageVM.Id);
            return pageVM;
        }

        public async Task<PageViewModel> AddPage(PageInsertModel pageInsertModel)
        {
            try
            {
                var pageEntity = mapper.Map<PageInsertModel, Page>(pageInsertModel);
                var result = uow.Add(pageEntity);
                await SaveAsync();
                if (result != null)
                {
                    pageEntity.Url = $"/{pageEntity.Id}";
                    uow.Update(pageEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Page, PageViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<PageViewModel> UpdatePage(PageUpdateModel pageUpdateModel)
        {
            try
            {
                var PageEntity = uow.Query<Page>().Where(x => x.Id == pageUpdateModel.Id).FirstOrDefault();
                if (PageEntity != null)
                {
                    PageEntity.NameAr = pageUpdateModel.NameAr;
                    PageEntity.DescriptionAr = pageUpdateModel.DescriptionAr;
                    PageEntity.DescriptionAr1 = pageUpdateModel.DescriptionAr1;
                    PageEntity.DescriptionAr2 = pageUpdateModel.DescriptionAr2;
                    PageEntity.DescriptionAr3 = pageUpdateModel.DescriptionAr3;
                    PageEntity.NameEn = pageUpdateModel.NameEn;
                    PageEntity.DescriptionEn = pageUpdateModel.DescriptionEn;
                    PageEntity.DescriptionEn1 = pageUpdateModel.DescriptionEn1;
                    PageEntity.DescriptionEn2 = pageUpdateModel.DescriptionEn2;
                    PageEntity.DescriptionEn3 = pageUpdateModel.DescriptionEn3;
                    PageEntity.NameGe = pageUpdateModel.NameGe;
                    PageEntity.DescriptionGe = pageUpdateModel.DescriptionGe;
                    PageEntity.DescriptionGe1 = pageUpdateModel.DescriptionGe1;
                    PageEntity.DescriptionGe2 = pageUpdateModel.DescriptionGe2;
                    PageEntity.DescriptionGe3 = pageUpdateModel.DescriptionGe3;
                    PageEntity.Type = pageUpdateModel.Type;
                    PageEntity.Image = pageUpdateModel.Image;
                    PageEntity.Image1 = pageUpdateModel.Image1;
                    PageEntity.Image2 = pageUpdateModel.Image2;
                    PageEntity.Image3 = pageUpdateModel.Image3;
                    PageEntity.EndpointAr = pageUpdateModel.EndpointAr;
                    PageEntity.EndpointEn = pageUpdateModel.EndpointEn;
                    PageEntity.EndpointGe = pageUpdateModel.EndpointGe;
                    PageEntity.RecordOrder = pageUpdateModel.RecordOrder;
                    PageEntity.IsActive = pageUpdateModel.IsActive;
                    PageEntity.Url = string.IsNullOrEmpty(PageEntity.Url) ? $"/{PageEntity.Id}" : pageUpdateModel.Url;
                    PageEntity.GeoLink = pageUpdateModel.GeoLink;
                    PageEntity.MenuId = pageUpdateModel.MenuId;

                    uow.Update(PageEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Page, PageViewModel>(PageEntity);
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

        public async Task<bool> SoftDeletePage(int pageId) // enable/disable
        {
            try
            {
                var pageEntity = uow.Query<Page>().Where(x => x.Id == pageId).FirstOrDefault();
                if (pageEntity != null)
                {
                    //pageEntity.IsActive = !pageEntity.IsActive;
                    uow.Remove(pageEntity);
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