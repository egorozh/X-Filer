using XFiler.Commands;
using XFiler.GoogleChromeStyle;
using XFiler.Resize;
using XFiler.SDK.Themes;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterSdkServices(this IDIService services)
    {
        services.RegisterIconServices();
        services.RegisterStartupOptions();
        services.RegisterReactiveOptions();
        services.RegisterThemes();
        services.RegisterBookmarksServices();
        
        services.RegisterSingleton<MainCommands, IMainCommands>();
        services.RegisterSingleton<ClipboardService, IClipboardService>();
        services.RegisterSingleton<FileOperations, IFileOperations>();
        services.RegisterSingleton<WindowsNaturalStringComparer, INaturalStringComparer>();
        services.RegisterSingleton<FileTypeResolver, IFileTypeResolver>();
        services.RegisterSingleton<RenameService, IRenameService>();
        services.RegisterSingleton<DirectorySettings, IDirectorySettings>();
        services.RegisterInitializeSingleton<LanguageService, ILanguageService>();
        services.RegisterInitializeSingleton<ThemeService, IThemeService>();
        services.RegisterInitializeSingleton<LaunchAtStartupService, ILaunchAtStartupService>();
        services.RegisterInitializeSingleton<DriveDetector, IDriveDetector>();
        services.RegisterSingleton<RestartService, IRestartService>();
        services.RegisterSingleton<WallpapersService, IWallpapersService>();
        services.RegisterInitializeSingleton<WindowsApiContextMenuLoader, INativeContextMenuLoader>();
    }

    private static void RegisterBookmarksServices(this IDIService services)
    {
        services.Register<MenuItemViewModel, IMenuItemViewModel>();

        services.RegisterSingleton<MenuItemFactory, IMenuItemFactory>();
        services.RegisterSingleton<BookmarksManager, IBookmarksManager>();
    }

    private static void RegisterIconServices(this IDIService services)
    {
        services.RegisterSingleton<FastResizeImageService, IResizeImageService>();

        // Image icon pipeline:
        services.RegisterSingleton<NativeExeIconProvider, IIconProvider>();
        services.RegisterSingleton<IconProviderForImages, IIconProvider>();
        services.RegisterSingleton<BaseIconProvider, IIconProvider>();
        services.RegisterSingleton<NativeFileIconProvider, IIconProvider>();
        services.RegisterSingleton<BlankIconProvider, IIconProvider>();

        services.RegisterSingleton<IconLoader, IIconLoader>();
    }

    private static void RegisterStartupOptions(this IDIService services)
    {
        services.RegisterSingleton<StartupOptionsFileManager, IStartupOptionsFileManager>();

        services.RegisterSingleton(s =>
            s.Resolve<IStartupOptionsFileManager>().InitOptions());
    }

    private static void RegisterReactiveOptions(this IDIService services)
    {
        services.RegisterSingleton<ReactiveOptionsFileManager, IReactiveOptionsFileManager>();

        services.RegisterSingleton(s =>
            s.Resolve<IReactiveOptionsFileManager>().InitOptions());
    }

    private static void RegisterThemes(this IDIService services)
    {
        services.RegisterSingleton<GoogleChromeTheme, ITheme>();
    }
}