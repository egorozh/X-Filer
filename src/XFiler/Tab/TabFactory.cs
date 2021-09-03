using System;
using System.IO;
using XFiler.SDK;

namespace XFiler
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

        public ITabItemModel? CreateExplorerTab(DirectoryInfo directoryInfo)
        {
            var route = new XFilerRoute(directoryInfo);
            var page = _pageFactory.CreatePage(route);

            if (page == null)
                return null;

            return new TabItemModel(_bookmarksManager, _pageFactory, route, page);
        }
        
        public ITabItemModel? CreateTab(XFilerRoute route)
        {
            var page = _pageFactory.CreatePage(route);

            if (page == null)
                return null;

            return new TabItemModel(_bookmarksManager, _pageFactory, route, page);
        }

        public ITabItemModel CreateMyComputerTab()
        {
            var route = SpecialRoutes.MyComputer;
            var page = _pageFactory.CreatePage(route);

            if (page == null)
                throw new ArgumentNullException($"My computer page is null");

            return new TabItemModel(_bookmarksManager, _pageFactory, route, page);
        }
    }
}