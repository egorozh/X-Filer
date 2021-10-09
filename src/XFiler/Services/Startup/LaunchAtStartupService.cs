using Microsoft.Win32;
using System.ComponentModel;

namespace XFiler;

internal class LaunchAtStartupService : ILaunchAtStartupService
{
    #region Private Fields

    private const string RegistryStartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
    private const string AppName = "X-Filer";

    private readonly ILogger _logger;
    private readonly IReactiveOptions _reactiveOptions;
    private readonly string _appPath;

    #endregion

    #region Constructor

    public LaunchAtStartupService(ILogger logger, IReactiveOptions reactiveOptions)
    {
        _logger = logger;
        _reactiveOptions = reactiveOptions;

        try
        {
            _appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ??
                       throw new Exception("Can't definition app path");
            _logger.Information($"AppPath: {_appPath}");
        }
        catch (Exception e)
        {
            _logger.Error(e, "LaunchAtStartupService");
            _appPath = string.Empty;
        }
    }

    #endregion

    #region Public Methods

    public void Init()
    {
        _reactiveOptions.PropertyChanged += ReactiveOptionsOnPropertyChanged;
    }

    #endregion

    #region Private Methods

    private void ReactiveOptionsOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IReactiveOptions.LaunchAtStartup))
            AddOrRemoveApplicationToStartup(_reactiveOptions.LaunchAtStartup);
    }

    private void AddOrRemoveApplicationToStartup(bool isAddToStartup)
        => SetStartup(_logger, AppName, _appPath, isAddToStartup);

    private static void SetStartup(ILogger logger, string appName, string appPath, bool enable)
    {
        try
        {
            using var rk = Registry.CurrentUser.OpenSubKey(RegistryStartupKey, true);

            if (rk != null)
            {
                if (enable)
                    rk.SetValue(appName, appPath);
                else
                    rk.DeleteValue(appName, false);
            }
        }
        catch (Exception e)
        {
            logger.Error(e, "SetStartup");
        }
    }

    #endregion
}