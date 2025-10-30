using System.Linq;
using SchoolV01.Shared.Constants.Localization;
using SchoolV01.Shared.Settings;

namespace SchoolV01.Client.Infrastructure.Settings
{
    public record ClientPreference : IPreference
    {
        public bool IsDarkMode { get; set; }
        public bool IsRTL { get; set; } = true;
        public bool IsDrawerOpen { get; set; }
        public string PrimaryColor { get; set; }
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "ar-SY";
    }
}