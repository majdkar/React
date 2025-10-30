using SchoolV01.Shared.ViewModels.Menus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IMenuCategoryService
    {
        Task<List<MenuCategoryViewModel>> GetMenuCategories();

        Task<List<MenuCategoryViewModel>> GetPagedMenuCategories(string searchString, string orderBy);

        Task<MenuCategoryViewModel> GetMenuCategoryByID(int menuCategoryId);

        Task<MenuCategoryViewModel> AddMenuCategory(MenuCategoryInsertModel menuCategoryInsertModel);

        Task<MenuCategoryViewModel> UpdateMenuCategory(MenuCategoryUpdateModel menuCategoryUpdateModel);

        Task<bool> SoftDeleteMenuCategory(int menuCategoryId);

        Task SaveAsync();
    }
}
