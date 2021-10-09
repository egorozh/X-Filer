using System.Windows.Markup;

namespace XFiler;

internal class LanguageService : ILanguageService
{
    private CultureInfo _current;

    public CultureInfo[] Languages { get; }

    public CultureInfo Current
    {
        get => _current;
        set
        {
            _current = value;
            SetCulture(value);
        }
    }

    public LanguageService(IStartupOptions startupOptions)
    {
        Languages = new[]
        {
            new CultureInfo("En-us"),
            new CultureInfo("Ru-ru"),
        };

        var userLang = startupOptions.CurrentLanguage;

        if (userLang != null)
            Current = Languages.First(l => l.Name == userLang);
        else
        {
            var defaultCulture = Languages.FirstOrDefault(l => l.Name == CultureInfo.CurrentCulture.Name);

            Current = defaultCulture ?? Languages.First();
        }
    }

    public void Init()
    {
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }

    public static void SetCulture(CultureInfo currentCulture)
    {
        CultureInfo.DefaultThreadCurrentCulture = currentCulture;
        CultureInfo.DefaultThreadCurrentUICulture = currentCulture;
        CultureInfo.CurrentCulture = currentCulture;
        CultureInfo.CurrentUICulture = currentCulture;
    }
}