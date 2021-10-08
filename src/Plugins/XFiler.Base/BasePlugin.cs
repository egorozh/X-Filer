using XFiler.Base.Icons;
using XFiler.SDK.Plugins;

namespace XFiler.Base;

public sealed class BasePlugin : IPlugin
{
    public void Load(IDIService services)
    {
        services.Register<GridFilesPresenterViewModel, IFilesPresenter>(PresenterType.Grid);
        services.Register<SmallIconsPresenterViewModel, IFilesPresenter>(PresenterType.SmallIcons);
        services.Register<RegularIconsPresenterViewModel, IFilesPresenter>(PresenterType.RegularIcons);
        services.Register<LargeIconsPresenterViewModel, IFilesPresenter>(PresenterType.LargeIcons);
        services.Register<TilesPresenterModel, IFilesPresenter>(PresenterType.Tiles);
        services.Register<ContentPresenterViewModel, IFilesPresenter>(PresenterType.Content);

        services.Register<GridFilesPresenterFactory, IFilesPresenterFactory>();
        services.Register<SmallIconsPresenterFactory, IFilesPresenterFactory>();
        services.Register<RegularIconsPresenterFactory, IFilesPresenterFactory>();
        services.Register<LargeIconsPresenterFactory, IFilesPresenterFactory>();
        services.Register<TilesPresenterFactory, IFilesPresenterFactory>();
        services.Register<ContentPresenterFactory, IFilesPresenterFactory>();

        services.RegisterSingleton<SvgIconProvider, IIconProvider>();
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