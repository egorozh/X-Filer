﻿using Autofac;
using XFiler.Commands;
using XFiler.GoogleChromeStyle;
using XFiler.Resize;
using XFiler.SDK.Themes;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterSdkServices(this IDIService services)
    {
        services.RegisterSingleton<Storage, IStorage>();

        services.RegisterBookmarksServices();

        services.RegisterSingleton<MainCommands, IMainCommands>();

        services.RegisterSingleton<ClipboardService, IClipboardService>();

        services.RegisterSingleton<FileOperations, IFileOperations>();
        services.RegisterSingleton<WindowsNaturalStringComparer, INaturalStringComparer>();

        services.RegisterSingleton<FileTypeResolver, IFileTypeResolver>();

        services.RegisterIconServices();

        services.RegisterSingleton<RenameService, IRenameService>();

        services.RegisterStartupOptions();
        services.RegisterReactiveOptions();

        services.RegisterSingleton<DirectorySettings, IDirectorySettings>();
        services.RegisterSingleton<LanguageService, ILanguageService>();

        services.RegisterThemes();

        services.RegisterSingleton<ThemeService, IThemeService>();
        services.RegisterSingleton<LaunchAtStartupService, ILaunchAtStartupService>();
    }

    private static void RegisterBookmarksServices(this IDIService services)
    {
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