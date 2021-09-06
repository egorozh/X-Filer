using System.Windows;
using Autofac;
using Dragablz;
using GongSolutions.Wpf.DragDrop;
using XFiler.Base;
using XFiler.DispatcherPage;
using XFiler.DragDrop;
using XFiler.MyComputer;
using XFiler.NotifyIcon;
using XFiler.SDK;

namespace XFiler
{
    internal class IoC
    {
        public IContainer Build()
        {
            var services = new ContainerBuilder();

            RegisterServices(services);

            return services.Build();
        }

        private static void RegisterServices(ContainerBuilder services)
        {
            services.RegisterExternalServices();
            services.RegisterSdkServices();


            services.RegisterType<FileViewModel>().Keyed<FileEntityViewModel>(EntityType.File);
            services.RegisterType<DirectoryViewModel>().Keyed<FileEntityViewModel>(EntityType.Directory);

            services.RegisterType<MyComputerPageModel>().Keyed<IPageModel>(PageType.MyComputer);
            services.RegisterType<SettingsPageModel>().Keyed<IPageModel>(PageType.Settings);
            services.RegisterType<BookmarksDispatcherPageModel>().Keyed<IPageModel>(PageType.BookmarksDispatcher);

            services.RegisterType<MainWindowTabClient>().As<IInterTabClient>().SingleInstance();


            services.RegisterType<BookmarksDispatcherDropTarget>().As<IBookmarksDispatcherDropTarget>()
                .SingleInstance();

            services.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();

            services.RegisterType<XFilerDragDrop>().As<IDropTarget>().SingleInstance();
            services.RegisterType<XFilerDragHandler>().As<IDragSource>().SingleInstance();

            services.RegisterType<FileEntityFactory>().As<IFileEntityFactory>().SingleInstance();

            services.RegisterType<ResultModelFactory>().As<IResultModelFactory>().SingleInstance();
            services.RegisterType<SearchHandler>().As<ISearchHandler>().SingleInstance();

            services.RegisterType<TabFactory>().As<ITabFactory>().SingleInstance();
            services.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();

            services.RegisterType<TabsFactory>().As<ITabsFactory>().SingleInstance();

            services.RegisterType<NotifyIconViewModel>().AsSelf().SingleInstance();

            services.RegisterModule<BasePlugin>();
        }
    }
}