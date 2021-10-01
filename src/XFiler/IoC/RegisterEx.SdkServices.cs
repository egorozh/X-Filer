using Autofac;
using XFiler.Commands;
using XFiler.GoogleChromeStyle;
using XFiler.Resize;
using XFiler.SDK.Themes;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterSdkServices(this ContainerBuilder services)
    {
        services.RegisterType<Storage>().As<IStorage>().SingleInstance();

        services.RegisterBookmarksServices();

        services.RegisterType<MainCommands>().As<IMainCommands>().SingleInstance();

        services.RegisterType<ClipboardService>().As<IClipboardService>().SingleInstance();

        services.RegisterType<FileOperations>().As<IFileOperations>().SingleInstance();
        services.RegisterType<WindowsNaturalStringComparer>().As<INaturalStringComparer>().SingleInstance();

        services.RegisterType<FileTypeResolver>().As<IFileTypeResolver>().SingleInstance();

        services.RegisterIconServices();

        services.RegisterType<RenameService>().As<IRenameService>().SingleInstance();

        services.RegisterStartupOptions();
        services.RegisterReactiveOptions();

        services.RegisterType<DirectorySettings>().As<IDirectorySettings>().SingleInstance();
        services.RegisterType<LanguageService>().As<ILanguageService>().SingleInstance();

        services.RegisterThemes();

        services.RegisterType<ThemeService>().As<IThemeService>().SingleInstance();
    }

    private static void RegisterBookmarksServices(this ContainerBuilder services)
    {
        services.RegisterType<MenuItemFactory>().As<IMenuItemFactory>().SingleInstance();
        services.RegisterType<BookmarksManager>().As<IBookmarksManager>().SingleInstance();
    }

    private static void RegisterIconServices(this ContainerBuilder services)
    {
        services.RegisterType<FastResizeImageService>().As<IResizeImageService>().SingleInstance();

        // Image icon pipeline:
        services.RegisterType<NativeExeIconProvider>().As<IIconProvider>().SingleInstance();
        services.RegisterType<IconProviderForImages>().As<IIconProvider>().SingleInstance();
        services.RegisterType<BaseIconProvider>().As<IIconProvider>().SingleInstance();
        services.RegisterType<NativeFileIconProvider>().As<IIconProvider>().SingleInstance();
        services.RegisterType<BlankIconProvider>().As<IIconProvider>().SingleInstance();

        services.RegisterType<IconLoader>().As<IIconLoader>().SingleInstance();
    }

    private static void RegisterStartupOptions(this ContainerBuilder services)
    {
        IStartupOptions startupOptions = new StartupOptions();

        services.RegisterInstance(startupOptions).As<IStartupOptions>().SingleInstance();
    }

    private static void RegisterReactiveOptions(this ContainerBuilder services)
    {
        services.RegisterType<ReactiveOptionsFileManager>().As<IReactiveOptionsFileManager>().SingleInstance();

        services.Register(s => s.Resolve<IReactiveOptionsFileManager>().GetOptions())
            .As<IReactiveOptions>()
            .SingleInstance();

        IStartupOptions startupOptions = new StartupOptions();

        services.RegisterInstance(startupOptions).As<IStartupOptions>().SingleInstance();
    }

    private static void RegisterThemes(this ContainerBuilder services)
    {
        services.RegisterInstance(new GoogleChromeTheme()).As<ITheme>().SingleInstance();
    }
}