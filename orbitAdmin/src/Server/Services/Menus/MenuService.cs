using AutoMapper;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Menus;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public MenuService(IUnitOfWork<int> uow, IMapper mapper )
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<MenuViewModel>> GetMenus()
        {
            var menuEntities = await uow.Query<Menu>().ToListAsync();
            var menusVM = mapper.Map<List<Menu>, List<MenuViewModel>>(menuEntities);
            foreach (var item in menusVM)
            {
                item.ParentName = GetMenuNameById(item.ParentId);
            }
            return menusVM;
        }

        public async Task<List<MenuViewModel>> GetPagedMenus(string searchString, string orderBy)
        {
            var menuEntities = await uow.Query<Menu>().ToListAsync();

            if (menuEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    menuEntities = menuEntities.Where(x => x.NameAr.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        menuEntities = [.. menuEntities.OrderBy(x => x.NameAr)];
                    if (orderBy.Contains("Type"))
                        menuEntities = [.. menuEntities.OrderBy(x => x.Type)];
                }
            }

            var menusVM = mapper.Map<List<Menu>, List<MenuViewModel>>(menuEntities);
            foreach (var item in menusVM)
            {
                item.ParentName = GetMenuNameById(item.ParentId);
            }

            return menusVM;
        }

        public async Task<List<MenuViewModel>> GetMenusByCategoryId(int categoryId)
        {
            var menuEntities = await uow.Query<Menu>().Where(x => x.CategoryId == categoryId).ToListAsync();
            var menusVM = mapper.Map<List<Menu>, List<MenuViewModel>>(menuEntities);
            foreach (var item in menusVM)
            {
                item.ParentName = GetMenuNameById(item.ParentId);
            }
            return menusVM;
        }

        public async Task<MenuViewModel> GetMenuById(int menuId)
        {
            var menuEntity = await uow.Query<Menu>().Where(x => x.Id == menuId).FirstOrDefaultAsync();
            var menuVM = mapper.Map<Menu, MenuViewModel>(menuEntity);
            //menuVM.Translations = await translationService.GetTranslationByMenuId(menuVM.Id);
            if (menuVM.ParentId != null)
            {
                menuVM.ParentName = (await GetMenuById(menuVM.ParentId ?? 0)).NameAr;
                menuVM.ParentName = GetMenuNameById(menuVM.ParentId);
            }
            return menuVM;
        }

        public async Task<MenuViewModel> AddMenu(MenuInsertModel menuInsertModel)
        {
            try
            {
                var menuEntity = mapper.Map<MenuInsertModel, Menu>(menuInsertModel);
                var result = uow.Add(menuEntity);
                await SaveAsync();
                if (result != null)
                {
                    //menuEntity.PageUrl = $"/{menuEntity.Id}";
                    uow.Update(menuEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Menu, MenuViewModel>(result);
                    resultVM.ParentName = GetMenuNameById(resultVM.ParentId);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MenuViewModel> UpdateMenu(MenuUpdateModel menuUpdateModel)
        {
            try
            {
                var menuEntity = uow.Query<Menu>().Where(x => x.Id == menuUpdateModel.Id).FirstOrDefault();
                if (menuEntity != null)
                {
                    menuEntity.NameAr = menuUpdateModel.NameAr;
                    menuEntity.DescriptionAr = menuUpdateModel.DescriptionAr;
                    menuEntity.NameEn = menuUpdateModel.NameEn;
                    menuEntity.DescriptionEn = menuUpdateModel.DescriptionEn;
                    menuEntity.NameGe = menuUpdateModel.NameGe;
                    menuEntity.DescriptionGe = menuUpdateModel.DescriptionGe;
                    menuEntity.Type = menuUpdateModel.Type;
                    menuEntity.Image = menuUpdateModel.Image;
                    menuEntity.File = menuUpdateModel.File;
                    menuEntity.PageUrl = menuUpdateModel.PageUrl;
                    menuEntity.Url = menuUpdateModel.Url;
                    menuEntity.LevelOrder = menuUpdateModel.LevelOrder;
                    menuEntity.IsActive = menuUpdateModel.IsActive;
                    menuEntity.IsHome = menuUpdateModel.IsHome;
                    menuEntity.IsFooter = menuUpdateModel.IsFooter;
                    menuEntity.IsHomeFooter = menuUpdateModel.IsHomeFooter;
                    menuEntity.ParentId = menuUpdateModel.ParentId;
                    menuEntity.PageUrl = string.IsNullOrEmpty(menuEntity.PageUrl) ? "" : menuUpdateModel.PageUrl;
                    //menuToUpdate.CategoryId = menuUpdateModel.CategoryId; // category is not changable
                    uow.Update(menuEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Menu, MenuViewModel>(menuEntity);
                    if(resultVM.ParentId != null)
                    {
                        resultVM.ParentName = GetMenuNameById(resultVM.ParentId);
                    }
                    else
                    {
                        resultVM.ParentName = "";
                    }
                    
                    
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

        public async Task<bool> SoftDeleteMenu(int menuId)
        {
            try
            {
                var menuEntity = uow.Query<Menu>().Where(x => x.Id == menuId).FirstOrDefault();
                if (menuEntity != null)
                {
                    menuEntity.IsActive = !menuEntity.IsActive;
                    uow.Remove(menuEntity);
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

        private string GetMenuNameById(int? menuId)
        {
            if (menuId == null)
                return "";
            var menuEntity =  uow.Query<Menu>().Where(x => x.Id == menuId).FirstOrDefault();
            if (menuEntity != null)
                return menuEntity.NameAr;
            return "";
        }
    }
}
