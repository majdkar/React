using SchoolV01.Shared.Settings;
using System.Threading.Tasks;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}