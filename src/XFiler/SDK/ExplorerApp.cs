using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Autofac;
using XFiler.GoogleChromeStyle;
using XFiler.SDK.Themes;

namespace XFiler.SDK
{
    internal class ExplorerApp : Application, IExplorerApp
    {
        #region Private Fields

        private ExplorerTheme? _currentTheme;

        #endregion

        #region Public Properties

        public IContainer Host { get; }

        public ExplorerWindow ExplorerWindow { get; private set; } = null!;
        
        #endregion

        #region Constructor

        public ExplorerApp()
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

        private void SetTheme(ExplorerTheme newTheme)
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