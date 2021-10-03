using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using SingleInstanceHelper;
using XFiler.NotifyIcon;

namespace XFiler;

internal sealed partial class App : IXFilerApp
{
    #region Private Fields

    private TaskbarIcon _notifyIcon = null!;

    #endregion

    #region Public Properties

    public IContainer Host { get; private set; } = null!;

    #endregion

    #region Protected Methods

    protected override void OnStartup(StartupEventArgs e)
    {
        var first = ApplicationActivator.LaunchOrReturn(OnNextInstanceRunned, e.Args);

        if (!first)
            Shutdown();

        Host = new IoC().Build();

        Host.Resolve<ILanguageService>().Init();
        Host.Resolve<IThemeService>().Init();
        Host.Resolve<ILaunchAtStartupService>().Init();

        LoadNotifyIconResourceDictionary();

        _notifyIcon = FindResource("NotifyIcon") as TaskbarIcon
                      ?? throw new NotImplementedException("NotifyIcon not found in Resources");

        _notifyIcon.DataContext = Host.Resolve<NotifyIconViewModel>();

#if DEBUG
        var windowFactory = Host.Resolve<IWindowFactory>();
        var window = windowFactory.GetWindowWithRootTab();

        window.Show();
#endif

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _notifyIcon.Dispose();
        base.OnExit(e);
    }

    #endregion

    #region Private Methods

    private void OnNextInstanceRunned(string[] commandArgs)
    {
        var window = Windows.OfType<IMainWindow>().FirstOrDefault();

        if (window != null)
            window.NormalizeAndActivate();
        else
            Host.Resolve<IWindowFactory>()
                .GetWindowWithRootTab()
                .Show();
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Host.Resolve<ILogger>()?.Error(e.Exception, "Global unhandled exception");
        e.Handled = true;
    }

    private void LoadNotifyIconResourceDictionary()
    {
        var resources = Resources.MergedDictionaries;

        if (LoadComponent(new Uri("/XFiler;component/Resources/NotifyIcon.xaml", UriKind.Relative))
            is ResourceDictionary resourceDict)
            resources.Add(resourceDict);
    }

    #endregion
}