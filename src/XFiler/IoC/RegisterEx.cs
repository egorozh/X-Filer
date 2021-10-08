using Autofac;
using XFiler.DispatcherPage;
using XFiler.MyComputer;
using XFiler.ViewModels;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterGroups(this IDIService services)
    {
        services.Register<FilesGroupOfNone, IFilesGroup>();
        services.Register<FilesGroupOfName, IFilesGroup>();
        services.Register<FilesGroupOfType, IFilesGroup>();
    }

    public static void RegisterPages(this IDIService services)
    {
        services.Register<MyComputerPageModel, IPageModel>(PageType.MyComputer);
        services.Register<SettingsPageModel, IPageModel>(PageType.Settings);
        services.Register<BookmarksDispatcherPageModel, IPageModel>(PageType.BookmarksDispatcher);
        services.Register<ExplorerPageModel, IPageModel>(PageType.Explorer);
        services.Register<SearchPageModel, IPageModel>(PageType.Search);

        services.RegisterSingleton<PageFactory, IPageFactory>();
    }

    public static void RegisterFileModels(this IDIService services)
    {
        services.Register<FileViewModel, FileEntityViewModel>(EntityType.File);

        services.Register<DirectoryViewModel, FileEntityViewModel>(EntityType.Directory);

        services.RegisterSingleton<FileEntityFactory, IFileEntityFactory>();
    }
}