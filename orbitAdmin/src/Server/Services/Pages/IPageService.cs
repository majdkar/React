using SchoolV01.Shared.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IPageService
    {
        Task<List<PageViewModel>> GetPages();

        Task<List<PageViewModel>> GetPagedPages(string searchString, string orderBy);

        Task<PageViewModel> GetPageByID(int pageId);
        Task<PageViewModel> GetPageByEndpoint(string Endpoint);

        Task<PageViewModel> AddPage(PageInsertModel pageInsertModel);

        Task<PageViewModel> UpdatePage(PageUpdateModel paggeUpdateModel);

        Task<bool> SoftDeletePage(int pageId);

        Task SaveAsync();
    }
}
