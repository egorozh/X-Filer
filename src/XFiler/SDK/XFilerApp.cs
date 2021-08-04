using Autofac;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Hardcodet.Wpf.TaskbarNotification;
using SingleInstanceHelper;
using XFiler.GoogleChromeStyle;
using XFiler.SDK.Themes;

namespace XFiler.SDK
{
    internal class XFilerApp : Application, IXFilerApp
    {
        #region Private Fields

        private XFilerTheme? _currentTheme;
        private TaskbarIcon _notifyIcon;

        #endregion

        #region Public Properties

        public IContainer Host { get; private set; }

        #endregion

        #region Protected Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            var first = ApplicationActivator.LaunchOrReturn(OnNextInstanceRunned, e.Args);

            if (!first)
                Shutdown();

            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            Host = new IoC().Build();
            
            CultureInfo currentCulture = new("Ru-ru");
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


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

        #endregion
    }
}