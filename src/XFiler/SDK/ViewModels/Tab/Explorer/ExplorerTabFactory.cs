using System.IO;

namespace XFiler.SDK
{
    public class ExplorerTabFactory : IExplorerTabFactory
    {
        private readonly IFilesPresenterFactory _filesPresenterFactory;
        private readonly IBookmarksManager _bookmarksManager;

        public ExplorerTabFactory(IFilesPresenterFactory filesPresenterFactory,
            IBookmarksManager bookmarksManager)
        {
            _filesPresenterFactory = filesPresenterFactory;
            _bookmarksManager = bookmarksManager;
        }

        public IExplorerTabItemViewModel CreateExplorerTab(DirectoryInfo directoryInfo)
            => new ExplorerTabItemViewModel(_filesPresenterFactory, _bookmarksManager, directoryInfo);

        public IExplorerTabItemViewModel CreateExplorerTab(string dirPath, string name)
            => new ExplorerTabItemViewModel(_filesPresenterFactory, _bookmarksManager, dirPath, name);

        public IExplorerTabItemViewModel CreateRootTab()
            => new ExplorerTabItemViewModel(_filesPresenterFactory, _bookmarksManager, IXFilerApp.RootName,
                IXFilerApp.RootName);
    }
}