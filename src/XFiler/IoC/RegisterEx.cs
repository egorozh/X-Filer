using Autofac;
using XFiler.DispatcherPage;
using XFiler.MyComputer;
using XFiler.ViewModels;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterGroups(this ContainerBuilder services)
    {
        services.RegisterType<FilesGroupOfNone>().As<IFilesGroup>().ExternallyOwned();
        services.RegisterType<FilesGroupOfName>().As<IFilesGroup>().ExternallyOwned();
        services.RegisterType<FilesGroupOfType>().As<IFilesGroup>().ExternallyOwned();
    }

    public static void RegisterPages(this ContainerBuilder services)
    {
        services.RegisterType<MyComputerPageModel>().Keyed<IPageModel>(PageType.MyComputer).ExternallyOwned();
        services.RegisterType<SettingsPageModel>().Keyed<IPageModel>(PageType.Settings).ExternallyOwned();
        services.RegisterType<BookmarksDispatcherPageModel>().Keyed<IPageModel>(PageType.BookmarksDispatcher).ExternallyOwned();
        services.RegisterType<ExplorerPageModel>().Keyed<IPageModel>(PageType.Explorer).ExternallyOwned();
        services.RegisterType<SearchPageModel>().Keyed<IPageModel>(PageType.Search).ExternallyOwned();

        services.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();
    }

    public static void RegisterFileModels(this ContainerBuilder services)
    {
        services.RegisterType<FileViewModel>()
            .Keyed<FileEntityViewModel>(EntityType.File)
            .ExternallyOwned();

        services.RegisterType<DirectoryViewModel>()
            .Keyed<FileEntityViewModel>(EntityType.Directory)
            .ExternallyOwned(); 

        services.RegisterType<FileEntityFactory>().As<IFileEntityFactory>().SingleInstance();
    }
}