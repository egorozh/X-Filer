using System.Collections.Generic;
using System.Linq;

namespace XFiler.SDK
{
    public class TabsFactory : ITabsFactory
    {
        private readonly ITabClient _tabClient;
        private readonly IExplorerTabFactory _explorerTabFactory;
        private readonly IWindowFactory _windowFactory;
        private readonly IBookmarksManager _bookmarksManager;
        private readonly ISettingsTabFactory _settingsFactory;

        public TabsFactory(ITabClient tabClient, IExplorerTabFactory explorerTabFactory,
            IWindowFactory windowFactory, IBookmarksManager bookmarksManager, ISettingsTabFactory settingsFactory)
        {
            _tabClient = tabClient;
            _explorerTabFactory = explorerTabFactory;
            _windowFactory = windowFactory;
            _bookmarksManager = bookmarksManager;
            _settingsFactory = settingsFactory;
        }
    
        public ITabsViewModel CreateTabsViewModel(IEnumerable<ITabItem> initItems)
            => new TabsViewModel(_tabClient, _explorerTabFactory, _windowFactory,
                _bookmarksManager, initItems, _settingsFactory);

        public ITabsViewModel CreateTabsViewModel()
            => new TabsViewModel(_tabClient, _explorerTabFactory, _windowFactory, _bookmarksManager,
                Enumerable.Empty<ExplorerTabItemViewModel>(), _settingsFactory);
    }
}