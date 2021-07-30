using System;
using System.Windows;
using Dragablz;

namespace XFiler.SDK
{
    public class BoundExampleInterTabClient : IInterTabClient, ITabClient
    {
        private readonly Func<ITabsFactory> _tabsFactory;

        public BoundExampleInterTabClient(Func<ITabsFactory> tabsFactory)
        {
            _tabsFactory = tabsFactory;
        }

        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            ExplorerWindow view = new();
            var model = _tabsFactory.Invoke().CreateTabsViewModel();
            view.DataContext = model;
            return new NewTabHost<Window>(view, view.InitMainView.InitialTabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window) 
            => TabEmptiedResponse.CloseWindowOrLayoutBranch;
    }
}