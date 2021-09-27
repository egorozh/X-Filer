using System.Windows.Markup;

namespace XFiler
{
    internal class LanguageService : ILanguageService
    {
        private readonly IStartupOptions _startupOptions;

        private string[] _availableCultures =
        {
            "En-us",
            "Ru-ru"
        };

        public LanguageService(IStartupOptions startupOptions)
        {
            _startupOptions = startupOptions;
        }

        public void Init()
        {
            var userLang = _startupOptions.CurrentLanguage;

            if (userLang != null) 
                SetCulture(userLang);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        public static void SetCulture(string culture)
        {
            CultureInfo currentCulture = new(culture);

            CultureInfo.DefaultThreadCurrentCulture = currentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = currentCulture;
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentCulture;
        }
    }
}