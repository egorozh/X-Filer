using Autofac;
using Dragablz;
using GongSolutions.Wpf.DragDrop;
using XFiler.DragDrop;
using XFiler.History;
using XFiler.SDK.Plugins;
using XFiler.TrayIcon;

namespace XFiler;

internal sealed class IoC
{
    public IContainer Build()
    {
        var services = new AutofacDiService();

        IStorage storage = new Storage();

        RegisterServices(services, storage, storage.GetPlugins());

        return services.Build();
    }

    private static void RegisterServices(IDIService services, IStorage storage, IEnumerable<IPlugin> plugins)
    {
        services.RegisterSingleton(_ => storage);

        foreach (var plugin in plugins)
            plugin.Load(services);

        services.RegisterExternalServices();
        services.RegisterSdkServices();
        services.RegisterGroups();
        services.RegisterSorting();
        services.RegisterPages();
        services.RegisterFileModels();

        services.RegisterSingleton<MainWindowTabClient, IInterTabClient>();

        services.RegisterSingleton<BookmarksDispatcherDropTarget, IBookmarksDispatcherDropTarget>();

        services.RegisterSingleton<WindowFactory, IWindowFactory>();

        services.RegisterSingleton<XFilerDragDrop, IDropTarget>();
        services.RegisterSingleton<XFilerDragHandler, IDragSource>();

        services.RegisterSingleton<ResultModelFactory, IResultModelFactory>();
        services.RegisterSingleton<SearchHandler, ISearchHandler>();

        services.Register<DirectoryHistory, IDirectoryHistory>();
        services.Register<TabItemModel, ITabItemModel>();
        services.RegisterSingleton<TabFactory, ITabFactory>();

        services.Register<TabsViewModel, ITabsViewModel>();
        services.RegisterSingleton<TabsFactory, ITabsFactory>();

        services.RegisterSingleton<TrayIconViewModel, TrayIconViewModel>();
    }
}