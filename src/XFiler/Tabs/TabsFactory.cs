using Dragablz;

namespace XFiler;

public sealed class TabsFactory : ITabsFactory
{
    private readonly IInterTabClient _tabClient;
    private readonly ITabFactory _tabFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly IBookmarksManager _bookmarksManager;
        
    public TabsFactory(IInterTabClient tabClient, ITabFactory tabFactory,
        IWindowFactory windowFactory, IBookmarksManager bookmarksManager)
    {
        _tabClient = tabClient;
        _tabFactory = tabFactory;
        _windowFactory = windowFactory;
        _bookmarksManager = bookmarksManager;
    }

    public ITabsViewModel CreateTabsViewModel(IEnumerable<ITabItemModel> initItems)
        => new TabsViewModel(_tabClient, _tabFactory, _windowFactory,
            _bookmarksManager, initItems);

    public ITabsViewModel CreateTabsViewModel()
        => new TabsViewModel(_tabClient, _tabFactory, _windowFactory, _bookmarksManager,
            Enumerable.Empty<ITabItemModel>());
}