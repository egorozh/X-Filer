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
        var services = new ContainerBuilder();

        RegisterServices(services, new[]
        {
            new BasePlugin()
        });

        return services.Build();
    }

    private static void RegisterServices(ContainerBuilder services, IEnumerable<IPlugin> plugins)
    {
        foreach (var plugin in plugins)
            plugin.Load(services);

        services.RegisterExternalServices();
        services.RegisterSdkServices();
        services.RegisterGroups();
        services.RegisterPages();
        services.RegisterFileModels();

        services.RegisterType<MainWindowTabClient>().As<IInterTabClient>().SingleInstance();

        services.RegisterType<BookmarksDispatcherDropTarget>().As<IBookmarksDispatcherDropTarget>()
            .SingleInstance();

        services.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();

        services.RegisterType<XFilerDragDrop>().As<IDropTarget>().SingleInstance();
        services.RegisterType<XFilerDragHandler>().As<IDragSource>().SingleInstance();

       

        services.RegisterType<ResultModelFactory>().As<IResultModelFactory>().SingleInstance();
        services.RegisterType<SearchHandler>().As<ISearchHandler>().SingleInstance();

        services.RegisterType<DirectoryHistory>().As<IDirectoryHistory>();
        services.RegisterType<TabItemModel>().As<ITabItemModel>();
        services.RegisterType<TabFactory>().As<ITabFactory>().SingleInstance();

        services.RegisterType<TabsViewModel>().As<ITabsViewModel>();
        services.RegisterType<TabsFactory>().As<ITabsFactory>().SingleInstance();

        services.RegisterType<NotifyIconViewModel>().AsSelf().SingleInstance();
    }
}