using System.IO;

namespace XFiler.SDK
{
    public class TabFactory : ITabFactory
    {
        private readonly IBookmarksManager _bookmarksManager;
        private readonly IPageFactory _pageFactory;

        public TabFactory(IPageFactory pageFactory,
            IBookmarksManager bookmarksManager)
        {
            _pageFactory = pageFactory;
            _bookmarksManager = bookmarksManager;
        }

        public ITabItemModel CreateExplorerTab(DirectoryInfo directoryInfo)
            => new TabItemModel(_bookmarksManager, _pageFactory, directoryInfo);

        public ITabItemModel CreateTab(XFilerRoute route)
            => new TabItemModel(_bookmarksManager, _pageFactory, route);

        public ITabItemModel CreateMyComputerTab()
            => new TabItemModel(_bookmarksManager, _pageFactory, SpecialRoutes.MyComputer);
    }
}