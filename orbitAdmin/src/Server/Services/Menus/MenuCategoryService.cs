using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Menus;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;

namespace SchoolV01.Application.Services
{
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public MenuCategoryService(IUnitOfWork<int> uow,IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<MenuCategoryViewModel>> GetMenuCategories()
        {
            var menuCategoriesEntities = await uow.Query<MenuCategory>().ToListAsync();
            var menuCategoriesVM = mapper.Map<List<MenuCategory>, List<MenuCategoryViewModel>>(menuCategoriesEntities);
            return menuCategoriesVM;
        }

        public async Task<List<MenuCategoryViewModel>> GetPagedMenuCategories(string searchString, string orderBy)
        {
            var menuCategoriesEntities = await uow.Query<MenuCategory>().ToListAsync();

            if (menuCategoriesEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    menuCategoriesEntities = menuCategoriesEntities.Where(x => x.NameAr.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        menuCategoriesEntities = menuCategoriesEntities.OrderBy(x => x.NameAr).ToList();
                }
            }

            var menuCategoriesVM = mapper.Map<List<MenuCategory>, List<MenuCategoryViewModel>>(menuCategoriesEntities);

            return menuCategoriesVM;
        }

        public async Task<MenuCategoryViewModel> GetMenuCategoryByID(int menuCategoryId)
        {
            var menuCategoriesEntity = await uow.Query<MenuCategory>().Where(x => x.Id == menuCategoryId).FirstOrDefaultAsync();
            var menuCategoryVM = mapper.Map<MenuCategory, MenuCategoryViewModel>(menuCategoriesEntity);
            return menuCategoryVM;
        }

        public async Task<MenuCategoryViewModel> AddMenuCategory(MenuCategoryInsertModel menuCategoryInsertModel)
        {
            try
            {
                var menuCategoriesEntity = mapper.Map<MenuCategoryInsertModel, MenuCategory>(menuCategoryInsertModel);
                var result = uow.Add(menuCategoriesEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<MenuCategory, MenuCategoryViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MenuCategoryViewModel> UpdateMenuCategory(MenuCategoryUpdateModel menuCategoryUpdateModel)
        {
            try
            {
                var menuCategoriesEntity = uow.Query<MenuCategory>().Where(x => x.Id == menuCategoryUpdateModel.Id).FirstOrDefault(); 
                if (menuCategoriesEntity != null)
                {
                    menuCategoriesEntity.NameAr = menuCategoryUpdateModel.NameAr;
                    menuCategoriesEntity.DescriptionAr = menuCategoryUpdateModel.DescriptionAr;
                    menuCategoriesEntity.NameEn = menuCategoryUpdateModel.NameEn;
                    menuCategoriesEntity.DescriptionEn = menuCategoryUpdateModel.DescriptionEn;
                    menuCategoriesEntity.NameGe = menuCategoryUpdateModel.NameGe;
                    menuCategoriesEntity.DescriptionGe = menuCategoryUpdateModel.DescriptionGe;
                    menuCategoriesEntity.IsActive = menuCategoryUpdateModel.IsActive;
                    menuCategoriesEntity.IsVisibleUser = menuCategoryUpdateModel.IsVisibleUser;
                    uow.Update(menuCategoriesEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<MenuCategory, MenuCategoryViewModel>(menuCategoriesEntity);
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

        public async Task<bool> SoftDeleteMenuCategory(int menuCategoryId) // enable/disable
        {
            try
            {
                var menuCategoriesEntity = uow.Query<MenuCategory>().Where(x => x.Id == menuCategoryId).FirstOrDefault(); 
                if (menuCategoriesEntity != null)
                {
                    menuCategoriesEntity.IsActive = !menuCategoriesEntity.IsActive;
                    uow.Update(menuCategoriesEntity);
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
