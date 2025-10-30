using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Localization;
public class LocalizationService
{
    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var type = typeof(LocalizationService);
        _localizer = factory.Create("Resources", type.Assembly.GetName().Name);
    }

    public IStringLocalizer GetLocalizer()
    {
        return _localizer;
    }
}
