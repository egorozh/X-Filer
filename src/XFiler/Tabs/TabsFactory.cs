namespace XFiler;

internal sealed class TabsFactory : ITabsFactory
{
    private readonly Func<ITabsViewModel> _tabsFactory;
   
    public TabsFactory(Func<ITabsViewModel> tabsFactory)
    {
        _tabsFactory = tabsFactory;
    }

    public ITabsViewModel CreateTabsViewModel()
        => CreateTabsViewModel(Enumerable.Empty<ITabItemModel>());

    public ITabsViewModel CreateTabsViewModel(IEnumerable<ITabItemModel> initTabs)
    {
        var tabs = _tabsFactory.Invoke();

        tabs.Init(initTabs);

        return tabs;
    }
}