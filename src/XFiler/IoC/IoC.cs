using Autofac;
using Dragablz;
using GongSolutions.Wpf.DragDrop;
using XFiler.Base;
using XFiler.DragDrop;
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

            services.RegisterType<MainWindowTabClient>().As<IInterTabClient>().SingleInstance();

            services.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();

            services.RegisterType<XFilerDragDrop>().As<IDropTarget>().SingleInstance();
            services.RegisterType<XFilerDragHandler>().As<IDragSource>().SingleInstance();

            services.RegisterType<FileEntityFactory>().As<IFileEntityFactory>().SingleInstance();

            services.RegisterType<TabFactory>().As<ITabFactory>().SingleInstance();
            services.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();

            services.RegisterType<TabsFactory>().As<ITabsFactory>().SingleInstance();

            services.RegisterType<NotifyIconViewModel>().AsSelf().SingleInstance();

            services.RegisterModule<BasePlugin>();
        }
    }
}