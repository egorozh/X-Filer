using Autofac.Features.Indexed;
using System.Diagnostics;
using System.IO;
using XFiler.Resources.Localization;
using XFiler.ViewModels;

namespace XFiler;

internal sealed class PageFactory : IPageFactory
{
    private readonly IIndex<PageType, IPageModel> _pageModelFactory;

    public PageFactory(IIndex<PageType, IPageModel> pageModelFactory)
    {
        _pageModelFactory = pageModelFactory;
    }

    public IPageModel CreatePage(Route route) => route.Type switch
    {
        RouteType.File => CreateFilePage(route),
        RouteType.WebLink => CreateWebPage(route),
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
        _ => CreateSearchPage(route)
    };

    private IPageModel CreateWebPage(Route route)
    {
        OpenFile(route.FullName);
        return new InvalidatePage(route);
    }

    private IPageModel CreateFilePage(Route route)
    {
        OpenFile(route.FullName);
        return new InvalidatePage(route);
    }

    private IPageModel CreateSearchPage(Route route)
    {
        var searchPage = (SearchPageModel) _pageModelFactory[PageType.Search];

        searchPage.Init(route);

        return searchPage;
    }

    private IPageModel CreateExplorerPage(Route route)
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

            return new InvalidatePage(route);
        }

        var explorerPage = (ExplorerPageModel) _pageModelFactory[PageType.Explorer];
        explorerPage.Init(dir);

        return explorerPage;
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