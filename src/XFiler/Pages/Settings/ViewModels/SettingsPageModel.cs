using System.ComponentModel;

namespace XFiler;

internal sealed class SettingsPageModel : BasePageModel, ISettingsPageModel
{
    private IStartupOptionsFileManager _startupOptionsFileManager;
    private IReactiveOptionsFileManager _reactiveOptionsFileManager;

    public IReactiveOptions ReactiveOptions { get; private set; }
    public StartupOptions StartupOptions { get; private set; }

    public CultureInfo[] Languages { get; }
    public CultureInfo CurrentLanguage { get; set; }

    public SettingsPageModel(IReactiveOptions reactiveOptions,
        IStartupOptions startupOptions,
        IStartupOptionsFileManager startupOptionsFileManager,
        IReactiveOptionsFileManager reactiveOptionsFileManager,
        ILanguageService languageService)
    {
        Init(typeof(SettingsPage), SpecialRoutes.Settings);

        Languages = languageService.Languages;
        CurrentLanguage = languageService.Current;


        ReactiveOptions = reactiveOptions;
        StartupOptions = (StartupOptions) startupOptions;
        _startupOptionsFileManager = startupOptionsFileManager;
        _reactiveOptionsFileManager = reactiveOptionsFileManager;

        ReactiveOptions.PropertyChanged += ReactiveOptionsOnPropertyChanged;
        StartupOptions.PropertyChanged += StartupOptionsOnPropertyChanged;

        PropertyChanged += SettingsPropertyChanged;
    }
    
    public override void Dispose()
    {
        base.Dispose();

        ReactiveOptions.PropertyChanged -= ReactiveOptionsOnPropertyChanged;
        StartupOptions.PropertyChanged -= StartupOptionsOnPropertyChanged;
        PropertyChanged -= SettingsPropertyChanged;

        ReactiveOptions = null!;
        StartupOptions = null!;
        _startupOptionsFileManager = null!;
        _reactiveOptionsFileManager = null!;
    }

    private async void StartupOptionsOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        await _startupOptionsFileManager.Save();

        MessageBox.Show("Изменения вступят в силу только после перезапуска приложения",
            "Изменения настроек", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void ReactiveOptionsOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        await _reactiveOptionsFileManager.Save();
    }

    private void SettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CurrentLanguage))
        {
            StartupOptions.CurrentLanguage = CurrentLanguage.Name;
        }
    }
}