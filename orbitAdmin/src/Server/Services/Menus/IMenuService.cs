using SchoolV01.Shared.ViewModels.Menus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IMenuService
    {
        Task<List<MenuViewModel>> GetMenus();

        Task<List<MenuViewModel>> GetPagedMenus(string searchString, string orderBy);

        Task<List<MenuViewModel>> GetMenusByCategoryId(int categoryId);

        Task<MenuViewModel> GetMenuById(int menuId);

        Task<MenuViewModel> AddMenu(MenuInsertModel menuInsertModel);

        Task<MenuViewModel> UpdateMenu(MenuUpdateModel menuUpdateModel);

        Task<bool> SoftDeleteMenu(int menuId);

        Task SaveAsync();
    }
}
