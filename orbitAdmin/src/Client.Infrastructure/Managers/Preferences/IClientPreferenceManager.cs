using SchoolV01.Shared.Managers;
using MudBlazor;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Preferences
{
    public interface IClientPreferenceManager : IPreferenceManager
    {

        Task<bool> ToggleDarkModeAsync();
    }
}