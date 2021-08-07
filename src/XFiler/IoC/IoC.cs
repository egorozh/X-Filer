using System.Collections.Generic;
using Autofac;
using GongSolutions.Wpf.DragDrop;
using XFiler.Base;
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
            var imageProviders = new List<IImageProvider>
            {
                // Always Last
                new BaseImageProvider()
            };
            services.RegisterType<IconLoader>().As<IIconLoader>().SingleInstance();
            services.RegisterInstance(imageProviders).As<IEnumerable<IImageProvider>>().SingleInstance();

            services.RegisterType<MenuItemFactory>().As<IMenuItemFactory>().SingleInstance();
            services.RegisterType<BookmarksManager>().As<IBookmarksManager>().SingleInstance();

            services.RegisterType<BoundExampleInterTabClient>().As<ITabClient>().SingleInstance();

            services.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();

            services.RegisterType<ChromerDragDrop>().As<IDropTarget>().SingleInstance();
            services.RegisterType<ChromerDragHandler>().As<IDragSource>().SingleInstance();

            services.RegisterType<FileEntityFactory>().As<IFileEntityFactory>().SingleInstance();

            services.RegisterType<TabFactory>().As<ITabFactory>().SingleInstance();
            services.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();
            services.RegisterType<SettingsTabFactory>().As<ISettingsTabFactory>().SingleInstance();

            services.RegisterType<TabsFactory>().As<ITabsFactory>().SingleInstance();

            services.RegisterType<NotifyIconViewModel>().AsSelf().SingleInstance();

            services.RegisterModule<BasePlugin>();
        }
    }
}