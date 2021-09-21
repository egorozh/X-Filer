using Autofac;
using XFiler.Commands;

namespace XFiler
{
    internal static class RegisterSdkServicesExtensions
    {
        public static void RegisterSdkServices(this ContainerBuilder services)
        {
            services.RegisterType<Storage>().As<IStorage>().SingleInstance();

            services.RegisterBookmarksServices();

            services.RegisterType<MainCommands>().As<IMainCommands>().SingleInstance();

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
            // Image icon pipeline
            var imageProviders = new List<IIconProvider>
            {
                // Other icon providers insert begin pipeline

                new NativeExeIconProvider(),
                new IconProviderForIcons(),
                new BaseIconProvider(),
                new NativeFileIconProvider(),
                new BlankIconProvider()
            };

            services.RegisterInstance(imageProviders).As<IEnumerable<IIconProvider>>().SingleInstance();

            services.RegisterType<IconLoader>().As<IIconLoader>().SingleInstance();
        }
    }
}