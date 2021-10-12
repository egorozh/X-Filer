using System.IO;
using System.Windows.Media;

namespace XFiler;

internal sealed class BaseIconProvider : IIconProvider
{
    public ImageSource? GetIcon(Route? route, IconSize size)
    {
        var key = GetResourceKey(route);

        return Application.Current.TryFindResource(key) as ImageSource;
    }

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
    {
        return null;
    }

    private static string GetResourceKey(Route? route)
    {
        if (route == null)
            return IconName.BookmarkFolder;

        if (route is FileRoute fileRoute)
            return GetExtensionKey(fileRoute.File.Extension);

        return route.Type switch
        {
            RouteType.MyComputer => IconName.MyComputer,
            RouteType.Desktop => IconName.Desktop,
            RouteType.Downloads => IconName.Downloads,
            RouteType.MyDocuments => IconName.MyDocuments,
            RouteType.MyMusic => IconName.MyMusic,
            RouteType.MyPictures => IconName.MyPictures,
            RouteType.MyVideos => IconName.MyVideos,
            RouteType.Settings => IconName.Settings,
            RouteType.Drive => IconName.LogicalDrive,
            RouteType.SystemDrive => IconName.SystemDrive,
            RouteType.Directory => IconName.Folder,
            RouteType.RecycleBin => IconName.RecycleBin,
            _ => IconName.Blank
        };
    }

    private static string GetExtensionKey(string extension) => string.IsNullOrWhiteSpace(extension)
        ? IconName.Blank
        : extension[1..].ToLower();
}

internal static class IconName
{
    public const string Blank = "_blank";
    public const string Folder = "_folder";
    public const string BookmarkFolder = "_bookmark_folder";
    public const string LogicalDrive = "_logicalDrive";
    public const string SystemDrive = "_systemDrive";


    public const string Desktop = "_desktop";
    public const string Downloads = "_downloads";
    public const string MyComputer = "_myComputer";
    public const string MyDocuments = "_myDocuments";
    public const string MyMusic = "_myMusic";
    public const string MyPictures = "_myPictures";
    public const string MyVideos = "_myVideos";
    public const string Settings = "_settings";
    public const string RecycleBin = "_recycleBin";
}