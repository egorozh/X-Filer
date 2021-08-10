using System.IO;
using System.Windows;
using System.Windows.Media;

namespace XFiler.SDK
{
    public class BaseImageProvider : IImageProvider
    {
        public ImageSource? GetIcon(XFilerRoute route, double size)
        {
            var key = GetResourceKey(route);

            ImageSource? source;

            if (Application.Current.TryFindResource(key) is ImageSource s)
                source = s;
            else
                source = Application.Current.TryFindResource(IconName.Blank) as ImageSource;

            return source;
        }

        private static string GetResourceKey(XFilerRoute? route)
        {
            if (route == null)
                return IconName.BookmarkFolder;

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
                RouteType.File => GetExtensionKey(route.FullName),
                _ => IconName.Blank
            };
        }

        private static string GetExtensionKey(string path)
        {
            var extension = new FileInfo(path).Extension;

            return string.IsNullOrWhiteSpace(extension)
                ? IconName.Blank
                : extension.Substring(1).ToLower();
        }

        private static class IconName
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
        }
    }
}