using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using Serilog;
using SingleInstanceHelper;
using System.Windows.Threading;
using XFiler.GoogleChromeStyle;
using XFiler.NotifyIcon;
using XFiler.SDK.Themes;

namespace XFiler
{
    internal sealed partial class App : IXFilerApp
    {
        #region Private Fields

        private XFilerTheme? _currentTheme;
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

            SetTheme(new GoogleChromeTheme());

            _notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");

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

        private void SetTheme(XFilerTheme newTheme)
        {
            if (_currentTheme != null)
            {
                var resourceDictionaryToRemove =
                    Resources.MergedDictionaries.FirstOrDefault(r => r.Source == _currentTheme.GetResourceUri());
                if (resourceDictionaryToRemove != null)
                    Resources.MergedDictionaries.Remove(resourceDictionaryToRemove);
            }

            _currentTheme = newTheme;

            if (LoadComponent(_currentTheme.GetResourceUri()) is ResourceDictionary resourceDict)
                Resources.MergedDictionaries.Add(resourceDict);
        }

        private void OnNextInstanceRunned(string[] commandArgs)
        {
            var window = Windows.OfType<IXFilerWindow>().FirstOrDefault();

            if (window != null)
            {
                window.NormalizeAndActivate();
            }
            else
            {
                var windowFactory = Host.Resolve<IWindowFactory>();

                windowFactory.GetWindowWithRootTab().Show();
            }
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Host.Resolve<ILogger>()?.Error(e.Exception, "Global unhandled exception");
            e.Handled = true;
        }

        #endregion
    }
}