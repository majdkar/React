using System.Globalization;
using System.Linq;

namespace SchoolV01.Shared.Constants.Localization
{
    public static class LocalizationConstants
    {
        public static readonly LanguageCode[] SupportedLanguages =
        {
            new ()
            {
                Code = "ar-SY",
                DisplayName= "العربية"
            },
            new ()
            {
                Code = "en-Us",
                DisplayName= "English"
            }


        };

        public static readonly CultureInfo DefaultCulture = new CultureInfo(SupportedLanguages.FirstOrDefault()?.Code ?? "ar-SY");
    };

}
