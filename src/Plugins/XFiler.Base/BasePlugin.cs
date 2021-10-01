using Autofac;
using XFiler.Base.Icons;
using XFiler.SDK.Plugins;

namespace XFiler.Base;

public sealed class BasePlugin : IPlugin
{
    public void Load(ContainerBuilder services)
    {
        services.RegisterType<GridFilesPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.Grid);
        services.RegisterType<SmallIconsPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.SmallIcons);
        services.RegisterType<RegularIconsPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.RegularIcons);
        services.RegisterType<LargeIconsPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.LargeIcons);
        services.RegisterType<TilesPresenterModel>().Keyed<IFilesPresenter>(PresenterType.Tiles);
        services.RegisterType<ContentPresenterViewModel>().Keyed<IFilesPresenter>(PresenterType.Content);


        services.RegisterType<GridFilesPresenterFactory>().As<IFilesPresenterFactory>();
        services.RegisterType<SmallIconsPresenterFactory>().As<IFilesPresenterFactory>();
        services.RegisterType<RegularIconsPresenterFactory>().As<IFilesPresenterFactory>();
        services.RegisterType<LargeIconsPresenterFactory>().As<IFilesPresenterFactory>();
        services.RegisterType<TilesPresenterFactory>().As<IFilesPresenterFactory>();
        services.RegisterType<ContentPresenterFactory>().As<IFilesPresenterFactory>();

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