using System.Linq;
using SchoolV01.Shared.Constants.Localization;
using SchoolV01.Shared.Settings;

namespace SchoolV01.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

        //TODO - add server preferences
    }
}