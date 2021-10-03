using System.ComponentModel;

namespace XFiler;

internal sealed class SettingsPageModel : BasePageModel, ISettingsPageModel
{
    private IStartupOptionsFileManager _startupOptionsFileManager;
    private IReactiveOptionsFileManager _reactiveOptionsFileManager;

    public IReactiveOptions ReactiveOptions { get; private set; }
    public IStartupOptions StartupOptions { get; private set; }

    public SettingsPageModel(IReactiveOptions reactiveOptions,
        IStartupOptions startupOptions,
        IStartupOptionsFileManager startupOptionsFileManager,
        IReactiveOptionsFileManager reactiveOptionsFileManager)
    {
        Init(typeof(SettingsPage), SpecialRoutes.Settings);

        ReactiveOptions = reactiveOptions;
        StartupOptions = startupOptions;
        _startupOptionsFileManager = startupOptionsFileManager;
        _reactiveOptionsFileManager = reactiveOptionsFileManager;

        ReactiveOptions.PropertyChanged += ReactiveOptionsOnPropertyChanged;
        StartupOptions.PropertyChanged += StartupOptionsOnPropertyChanged;
    }

    public override void Dispose()
    {
        base.Dispose();

        ReactiveOptions.PropertyChanged -= ReactiveOptionsOnPropertyChanged;
        StartupOptions.PropertyChanged -= StartupOptionsOnPropertyChanged;

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
}