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

        public TabsFactory(ITabClient tabClient, IExplorerTabFactory explorerTabFactory,
            IWindowFactory windowFactory, IBookmarksManager bookmarksManager)
        {
            _tabClient = tabClient;
            _explorerTabFactory = explorerTabFactory;
            _windowFactory = windowFactory;
            _bookmarksManager = bookmarksManager;
        }
    
        public ITabsViewModel CreateTabsViewModel(IEnumerable<ITabItem> initItems)
            => new TabsViewModel(_tabClient, _explorerTabFactory, _windowFactory, _bookmarksManager, initItems);

        public ITabsViewModel CreateTabsViewModel()
            => new TabsViewModel(_tabClient, _explorerTabFactory, _windowFactory, _bookmarksManager,
                Enumerable.Empty<ExplorerTabItemViewModel>());
    }
}