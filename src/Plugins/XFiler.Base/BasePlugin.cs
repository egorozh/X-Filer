using Autofac;
using XFiler.Base.Icons;
using XFiler.SDK.Plugins;

namespace XFiler.Base;

public sealed class BasePlugin : IPlugin
{
    public void Load(ContainerBuilder services)
    {
        services.RegisterType<GridFilesPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.Grid).ExternallyOwned();
        services.RegisterType<SmallIconsPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.SmallIcons).ExternallyOwned();
        services.RegisterType<RegularIconsPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.RegularIcons).ExternallyOwned();
        services.RegisterType<LargeIconsPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.LargeIcons).ExternallyOwned();
        services.RegisterType<TilesPresenterModel>().Keyed<IFilesPresenter>(PresenterType.Tiles).ExternallyOwned();
        services.RegisterType<ContentPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.Content).ExternallyOwned();


        services.RegisterType<GridFilesPresenterFactory>().As<IFilesPresenterFactory>().ExternallyOwned();
        services.RegisterType<SmallIconsPresenterFactory>().As<IFilesPresenterFactory>().ExternallyOwned();
        services.RegisterType<RegularIconsPresenterFactory>().As<IFilesPresenterFactory>().ExternallyOwned();
        services.RegisterType<LargeIconsPresenterFactory>().As<IFilesPresenterFactory>().ExternallyOwned();
        services.RegisterType<TilesPresenterFactory>().As<IFilesPresenterFactory>().ExternallyOwned();
        services.RegisterType<ContentPresenterFactory>().As<IFilesPresenterFactory>().ExternallyOwned();

        services.RegisterType<SvgIconProvider>().As<IIconProvider>().SingleInstance();
    }
}

internal enum PresenterType
{
    Grid,

    SmallIcons,

    RegularIcons,

    LargeIcons,

    Tiles,

    Content
}