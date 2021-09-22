using Autofac.Features.Indexed;
using System.Diagnostics;
using System.IO;
using XFiler.Resources.Localization;
using XFiler.ViewModels;

namespace XFiler
{
    internal sealed class PageFactory : IPageFactory
    {
        private readonly IIndex<PageType, IPageModel> _pageModelFactory;
        private readonly Func<IReadOnlyList<IFilesPresenterFactory>> _filesPresenters;
        private readonly Func<IReadOnlyList<IFilesGroup>> _groups;
        private readonly IClipboardService _clipboardService;
        private readonly IMainCommands _mainCommands;

        public PageFactory(
            IIndex<PageType, IPageModel> pageModelFactory,
            Func<IReadOnlyList<IFilesPresenterFactory>> filesPresenters,
            Func<IReadOnlyList<IFilesGroup>> groups,
            IClipboardService clipboardService,
            IMainCommands mainCommands)
        {
            _pageModelFactory = pageModelFactory;
            _filesPresenters = filesPresenters;
            _groups = groups;
            _clipboardService = clipboardService;
            _mainCommands = mainCommands;
        }

        public IPageModel? CreatePage(XFilerRoute route)
        {
            switch (route.Type)
            {
                case RouteType.File:
                    OpenFile(route.FullName);
                    return null;
                case RouteType.WebLink:
                    OpenFile(route.FullName);
                    return null;
                default:
                    return route.Type switch
                    {
                        RouteType.Directory => CreateExplorerPage(route),
                        RouteType.Desktop => CreateExplorerPage(route),
                        RouteType.Downloads => CreateExplorerPage(route),
                        RouteType.MyDocuments => CreateExplorerPage(route),
                        RouteType.MyMusic => CreateExplorerPage(route),
                        RouteType.MyPictures => CreateExplorerPage(route),
                        RouteType.MyVideos => CreateExplorerPage(route),
                        RouteType.SystemDrive => CreateExplorerPage(route),
                        RouteType.Drive => CreateExplorerPage(route),
                        RouteType.RecycleBin => CreateExplorerPage(route),
                        RouteType.MyComputer => _pageModelFactory[PageType.MyComputer],
                        RouteType.Settings => _pageModelFactory[PageType.Settings],
                        RouteType.BookmarksDispatcher => _pageModelFactory[PageType.BookmarksDispatcher],
                        _ => new SearchPageModel(route)
                    };
            }
        }

        private ExplorerPageModel? CreateExplorerPage(XFilerRoute route)
        {
            var dir = new DirectoryInfo(route.FullName);

            try
            {
                var access = dir.EnumerateFileSystemInfos();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"{Strings.PageFactory_NotAccessText} \"{dir.FullName}\"",
                    Strings.PageFactory_NotAccessCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return null;
            }

            return new ExplorerPageModel(
                _filesPresenters.Invoke(),
                _groups.Invoke(),
                _clipboardService,
                _mainCommands,
                dir);
        }

        private static void OpenFile(string path)
        {
            try
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo(path)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }
    }
}