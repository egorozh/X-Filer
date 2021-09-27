using System.Linq;
using XFiler.GoogleChromeStyle;
using XFiler.SDK.Themes;

namespace XFiler
{
    internal class ThemeService : IThemeService
    {
        private readonly IReadOnlyList<ITheme> _themes;
        private readonly IReactiveOptions _reactiveOptions;

        private ITheme? _currentTheme;

        public ThemeService(IReadOnlyList<ITheme> themes, IReactiveOptions reactiveOptions)
        {
            _themes = themes;
            _reactiveOptions = reactiveOptions;
        }

        public void Init()
        {
            var theme = _themes.FirstOrDefault(t => t.Guid == _reactiveOptions.CurrentThemeId);

            if (theme != null) 
                SetTheme(theme);
        }

        private void SetTheme(ITheme newTheme)
        {
            if (_currentTheme != null)
            {
                var resourceDictionaryToRemove =
                    Application.Current.Resources.MergedDictionaries.FirstOrDefault(r =>
                        r.Source == _currentTheme.GetResourceUri());
                if (resourceDictionaryToRemove != null)
                    Application.Current.Resources.MergedDictionaries.Remove(resourceDictionaryToRemove);
            }

            _currentTheme = newTheme;

            if (Application.LoadComponent(_currentTheme.GetResourceUri()) is ResourceDictionary resourceDict)
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}