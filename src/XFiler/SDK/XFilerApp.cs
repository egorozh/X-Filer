using Autofac;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using XFiler.GoogleChromeStyle;
using XFiler.SDK.Themes;

namespace XFiler.SDK
{
    internal class XFilerApp : Application, IXFilerApp
    {
        #region Private Fields

        private XFilerTheme? _currentTheme;

        #endregion

        #region Public Properties

        public IContainer Host { get; }

        public ExplorerWindow ExplorerWindow { get; private set; } = null!;
        
        #endregion

        #region Constructor

        public XFilerApp()
        {
            Host = new IoC().Build();
            
            CultureInfo currentCulture = new("Ru-ru");
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        #endregion

        #region Protected Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetTheme(new GoogleChromeTheme());

            ITabsFactory tabsFactory = Host.Resolve<ITabsFactory>();
            IExplorerTabFactory explorerTabFactory = Host.Resolve<IExplorerTabFactory>();

            var tabsViewModel = tabsFactory.CreateTabsViewModel(new[]
            {
                explorerTabFactory.CreateRootTab()
            });

            ExplorerWindow = new ExplorerWindow
            {
                DataContext = tabsViewModel
            };

            ExplorerWindow.Show();
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

        #endregion
    }
}