using Autofac;
using XFiler.DispatcherPage;
using XFiler.MyComputer;
using XFiler.ViewModels;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterGroups(this ContainerBuilder services)
    {
        services.RegisterType<FilesGroupOfNone>().As<IFilesGroup>();
        services.RegisterType<FilesGroupOfName>().As<IFilesGroup>();
        services.RegisterType<FilesGroupOfType>().As<IFilesGroup>();
    }

    public static void RegisterPages(this ContainerBuilder services)
    {
        services.RegisterType<MyComputerPageModel>().Keyed<IPageModel>(PageType.MyComputer);
        services.RegisterType<SettingsPageModel>().Keyed<IPageModel>(PageType.Settings);
        services.RegisterType<BookmarksDispatcherPageModel>().Keyed<IPageModel>(PageType.BookmarksDispatcher);
        services.RegisterType<ExplorerPageModel>().Keyed<IPageModel>(PageType.Explorer);
        services.RegisterType<SearchPageModel>().Keyed<IPageModel>(PageType.Search);
    }
}