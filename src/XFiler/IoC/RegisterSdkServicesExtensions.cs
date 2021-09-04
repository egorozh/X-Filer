using Autofac;
using System.Collections.Generic;
using XFiler.SDK;

namespace XFiler
{
    internal static class RegisterSdkServicesExtensions
    {
        public static void RegisterSdkServices(this ContainerBuilder services)
        {
            services.RegisterType<Storage>().As<IStorage>().SingleInstance();

            services.RegisterBookmarksServices();

            services.RegisterType<ClipboardService>().As<IClipboardService>().SingleInstance();
            
            services.RegisterType<FileOperations>().As<IFileOperations>().SingleInstance();

            services.RegisterIconServices();

            services.RegisterType<RenameService>().As<IRenameService>().SingleInstance();

            services.RegisterType<ExplorerOptions>().As<IExplorerOptions>().SingleInstance();
        }

        private static void RegisterBookmarksServices(this ContainerBuilder services)
        {
            services.RegisterType<MenuItemFactory>().As<IMenuItemFactory>().SingleInstance();
            services.RegisterType<BookmarksManager>().As<IBookmarksManager>().SingleInstance();
        }

        private static void RegisterIconServices(this ContainerBuilder services)
        {
            var imageProviders = new List<IImageProvider>
            {
                // Always Last
                new BaseImageProvider()
            };
            services.RegisterType<IconLoader>().As<IIconLoader>().SingleInstance();
            services.RegisterInstance(imageProviders).As<IEnumerable<IImageProvider>>().SingleInstance();
        }
    }
}