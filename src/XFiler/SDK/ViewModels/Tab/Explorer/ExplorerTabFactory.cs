using System;
using System.Collections.Generic;
using System.IO;

namespace XFiler.SDK
{
    public class ExplorerTabFactory : IExplorerTabFactory
    {
        private readonly IBookmarksManager _bookmarksManager;
        private readonly Func<IReadOnlyList<IFilesPresenterFactory>> _filesPresenterFactory;

        public ExplorerTabFactory(Func<IReadOnlyList<IFilesPresenterFactory>> filesPresenterFactory,
            IBookmarksManager bookmarksManager)
        {
            _filesPresenterFactory = filesPresenterFactory;
            _bookmarksManager = bookmarksManager;
        }

        public IExplorerTabItemViewModel CreateExplorerTab(DirectoryInfo directoryInfo)
            => new ExplorerTabItemViewModel(_filesPresenterFactory.Invoke(), _bookmarksManager, directoryInfo);

        public IExplorerTabItemViewModel CreateExplorerTab(string dirPath, string name)
            => new ExplorerTabItemViewModel(_filesPresenterFactory.Invoke(), _bookmarksManager, dirPath, name);

        public IExplorerTabItemViewModel CreateRootTab()
            => new ExplorerTabItemViewModel(_filesPresenterFactory.Invoke(), _bookmarksManager, IXFilerApp.RootName,
                IXFilerApp.RootName);
    }
}