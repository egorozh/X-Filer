using Autofac;
using Dragablz;
using GongSolutions.Wpf.DragDrop;
using XFiler.Base;
using XFiler.DragDrop;
using XFiler.History;
using XFiler.NotifyIcon;
using XFiler.SDK.Plugins;

namespace XFiler;

internal sealed class IoC
{
    public IContainer Build()
    {
        var services = new AutofacDIService();
        
        RegisterServices(services, new[]
        {
            new BasePlugin()
        });

        return services.Build();
    }

    private static void RegisterServices(IDIService services, IEnumerable<IPlugin> plugins)
    {   
        foreach (var plugin in plugins)
            plugin.Load(services);

        services.RegisterExternalServices();
        services.RegisterSdkServices();
        services.RegisterGroups();
        services.RegisterPages();
        services.RegisterFileModels();

        services.RegisterSingleton<MainWindowTabClient,IInterTabClient>();

        services.RegisterSingleton<BookmarksDispatcherDropTarget, IBookmarksDispatcherDropTarget>();
       
        services.RegisterSingleton<WindowFactory, IWindowFactory>();

        services.RegisterSingleton<XFilerDragDrop, IDropTarget>();
        services.RegisterSingleton<XFilerDragHandler, IDragSource>();

        services.RegisterSingleton<ResultModelFactory, IResultModelFactory>();
        services.RegisterSingleton<SearchHandler, ISearchHandler>();
        
        services.Register<DirectoryHistory,IDirectoryHistory>();
        services.Register<TabItemModel,ITabItemModel>();
        services.RegisterSingleton<TabFactory, ITabFactory>();

        services.Register<TabsViewModel, ITabsViewModel>();
        services.RegisterSingleton<TabsFactory, ITabsFactory>();

        services.RegisterSingleton<NotifyIconViewModel, NotifyIconViewModel>();
    }
}