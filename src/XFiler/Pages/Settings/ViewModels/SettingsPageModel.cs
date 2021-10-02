namespace XFiler;

internal sealed class SettingsPageModel : BasePageModel, ISettingsPageModel
{
    private IReactiveOptions _reactiveOptions;
    private IStartupOptions _startupOptions;
    private IStartupOptionsFileManager _startupOptionsFileManager;
    private IReactiveOptionsFileManager _reactiveOptionsFileManager;

    public DelegateCommand SaveCommand { get; }

    public SettingsPageModel(IReactiveOptions reactiveOptions,
        IStartupOptions startupOptions,
        IStartupOptionsFileManager startupOptionsFileManager,
        IReactiveOptionsFileManager reactiveOptionsFileManager)
    {
        Init(typeof(SettingsPage), SpecialRoutes.Settings);

        _reactiveOptions = reactiveOptions;
        _startupOptions = startupOptions;
        _startupOptionsFileManager = startupOptionsFileManager;
        _reactiveOptionsFileManager = reactiveOptionsFileManager;
        SaveCommand = new DelegateCommand(OnSave);
    }

    public override void Dispose()
    {
        base.Dispose();

        _reactiveOptions = null!;
        _startupOptions = null!;
        _startupOptionsFileManager = null!;
        _reactiveOptionsFileManager = null!;
    }

    private async void OnSave()
    {
        await _startupOptionsFileManager.Save();
        await _reactiveOptionsFileManager.Save();
    }
}