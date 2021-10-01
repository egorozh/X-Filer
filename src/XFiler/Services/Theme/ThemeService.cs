using System.ComponentModel;
using XFiler.SDK.Themes;

namespace XFiler;

internal sealed class ThemeService : IThemeService
{
    #region Private Fields

    private ITheme? _currentTheme;

    private readonly IReadOnlyList<ITheme> _themes;
    private readonly IReactiveOptions _reactiveOptions;

    #endregion

    #region Constructor

    public ThemeService(IReadOnlyList<ITheme> themes, IReactiveOptions reactiveOptions)
    {
        _themes = themes;
        _reactiveOptions = reactiveOptions;

        _reactiveOptions.PropertyChanged += ReactiveOptionsOnPropertyChanged;
    }

    #endregion

    #region Public Methods

    public void Init()
    {
        var theme = _themes.FirstOrDefault(t => t.Id == _reactiveOptions.CurrentThemeId);

        SetTheme(theme ?? _themes.First());
    }

    #endregion

    #region Private Methods

    private void SetTheme(ITheme newTheme)
    {
        if (newTheme == _currentTheme)
            return;

        var resources = Application.Current.Resources.MergedDictionaries;

        if (_currentTheme != null)
        {
            var resourceDictionaryToRemove = resources
                .FirstOrDefault(r => r.Source == _currentTheme.ResourceUri);

            if (resourceDictionaryToRemove != null)
                resources.Remove(resourceDictionaryToRemove);
        }

        _currentTheme = newTheme;

        if (Application.LoadComponent(_currentTheme.ResourceUri) is ResourceDictionary resourceDict)
            resources.Add(resourceDict);
    }

    private void ReactiveOptionsOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IReactiveOptions.CurrentThemeId))
        {
            Init();
        }
    }

    #endregion
}