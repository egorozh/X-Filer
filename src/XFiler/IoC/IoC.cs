using Autofac;
using GongSolutions.Wpf.DragDrop;
using XFiler.DragDrop;
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
            services.RegisterType<ExtensionToImageFileConverter>().AsSelf().SingleInstance();
            services.RegisterType<IconPathProvider>().As<IIconPathProvider>().SingleInstance();
            services.RegisterType<BookmarksManager>().As<IBookmarksManager>().SingleInstance();

            services.RegisterType<BoundExampleInterTabClient>().As<ITabClient>().SingleInstance();

            services.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();

            services.RegisterType<ChromerDragDrop>().As<IDropTarget>().SingleInstance();
            services.RegisterType<ChromerDragHandler>().As<IDragSource>().SingleInstance();
            services.RegisterType<IconLoader>().As<IIconLoader>().SingleInstance();
            services.RegisterType<FileEntityFactory>().As<IFileEntityFactory>().SingleInstance();
            services.RegisterType<FilesPresenterFactory>().As<IFilesPresenterFactory>().SingleInstance();
            services.RegisterType<ExplorerTabFactory>().As<IExplorerTabFactory>().SingleInstance();
            services.RegisterType<TabsFactory>().As<ITabsFactory>().SingleInstance();


            services.RegisterType<NotifyIconViewModel>().AsSelf().SingleInstance();
        }
    }
}