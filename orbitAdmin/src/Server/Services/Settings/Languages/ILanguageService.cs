using SchoolV01.Shared.ViewModels.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface ILanguageService
    {
        Task<List<LanguageViewModel>> GetLanguages();

        Task<LanguageViewModel> GetLanguageById(int languageId);

        Task<LanguageViewModel> AddLanguage(LanguageInsertModel languageInsertModel);

        Task<LanguageViewModel> UpdateLanguage(LanguageUpdateModel languageUpdateModel);

        Task<bool> SoftDeleteLanguage(int languageId);

        Task SaveAsync();
    }
}
