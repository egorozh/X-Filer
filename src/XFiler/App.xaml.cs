using System.Linq;
using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using SingleInstanceHelper;
using System.Windows.Markup;
using System.Windows.Threading;
using Serilog;
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

            //var availableCultures = new[]
            //{
            //    "Ru-ru",
            //    "En-us"
            //};

            //SetCulture(availableCultures[1]);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            Host = new IoC().Build();

            SetTheme(new GoogleChromeTheme());

            _notifyIcon.DataContext = Host.Resolve<NotifyIconViewModel>();

            var windowFactory = Host.Resolve<IWindowFactory>();

            var window = windowFactory.GetWindowWithRootTab();

            window.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            base.OnExit(e);
        }

        #endregion

        #region Public Methods

        public static void SetCulture(string culture)
        {
            CultureInfo currentCulture = new(culture);

            CultureInfo.DefaultThreadCurrentCulture = currentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = currentCulture;
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentCulture;
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