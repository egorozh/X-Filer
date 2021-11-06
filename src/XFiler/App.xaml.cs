using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using SingleInstanceHelper;
using System.Text;
using XFiler.SDK.Plugins;
using XFiler.TrayIcon;

namespace XFiler;

internal sealed partial class App : IXFilerApp
{
    #region Private Fields

    private TaskbarIcon _trayIcon = null!;

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

        var plugins = Host.Resolve<IReadOnlyList<IPlugin>>();

        foreach (var s in Host.Resolve<IReadOnlyList<IInitializeService>>()) 
            s.Init();
        
        LoadNotifyIconResourceDictionary();

        _trayIcon = FindResource("TrayIcon") as TaskbarIcon
                      ?? throw new NotImplementedException("TrayIcon not found in Resources");

        _trayIcon.DataContext = Host.Resolve<TrayIconViewModel>();

#if RELEASE
        if (e.Args.Length > 0 && e.Args[0].StartsWith(IRestartService.RestartKey))
        {
            Host.Resolve<IWindowFactory>()
                .GetWindowWithRootTab()
                .Show();
        }
#endif
        
#if DEBUG
        Host.Resolve<IWindowFactory>()
            .GetWindowWithRootTab()
            .Show();
#endif

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon.Dispose();
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

        if (LoadComponent(new Uri("/XFiler;component/Resources/TrayIcon.xaml", UriKind.Relative))
            is ResourceDictionary resourceDict)
            resources.Add(resourceDict);
    }
    
    private void ShowArgs(IEnumerable<string> args)
    {
        StringBuilder sb = new();

        foreach (var arg in args)
            sb.AppendLine(arg);

        MessageBox.Show(sb.ToString(), "Args");
    }

    #endregion
}