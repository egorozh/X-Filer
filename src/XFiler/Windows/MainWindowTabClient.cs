using Dragablz;

namespace XFiler;

public sealed class MainWindowTabClient : IInterTabClient
{
    private readonly Func<ITabsFactory> _tabsFactory;

    public MainWindowTabClient(Func<ITabsFactory> tabsFactory)
    {
        _tabsFactory = tabsFactory;
    }

    public INewTabHost<Window> GetNewHost(
        IInterTabClient interTabClient,
        object partition,
        TabablzControl source)
    {
        MainWindow view = new()
        {
            DataContext = _tabsFactory.Invoke().CreateTabsViewModel()
        };

        return new NewTabHost<Window>(view, view.InitMainView.InitialTabablzControl);
    }

    public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        => TabEmptiedResponse.CloseWindowOrLayoutBranch;
}