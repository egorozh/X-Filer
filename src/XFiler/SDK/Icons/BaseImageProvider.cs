using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace XFiler.SDK
{
    public class BaseImageProvider : IImageProvider
    {
        public ImageSource? GetIcon(FileEntityViewModel viewModel, double size)
        {
            var key = GetResourceKey(viewModel);

            ImageSource? source;

            if (Application.Current.TryFindResource(key) is ImageSource s)
                source = s;
            else
                source = Application.Current.TryFindResource(IconName.Blank) as ImageSource;

            return source;
        }
        
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

        private static string GetResourceKey(FileEntityViewModel viewModel) => viewModel switch
        {
            FileViewModel fileViewModel => GetExtensionKey(fileViewModel.FullName),
            LogicalDriveViewModel { FullName: "C:\\" } => IconName.SystemDrive,
            LogicalDriveViewModel => IconName.LogicalDrive,
            DirectoryViewModel => IconName.Folder,
            _ => IconName.Blank
        };

        private static string GetResourceKey(XFilerRoute? route)
        {
            if (route == null)
                return IconName.BookmarkFolder;

            if (route.Type == RouteType.Special)
            {
                if (route == SpecialRoutes.Desktop)
                    return IconName.Desktop;
                if (route == SpecialRoutes.Downloads)
                    return IconName.Downloads;
                if (route == SpecialRoutes.MyComputer)
                    return IconName.MyComputer;
                if (route == SpecialRoutes.MyDocuments)
                    return IconName.MyDocuments;
                if (route == SpecialRoutes.MyMusic)
                    return IconName.MyMusic;
                if (route == SpecialRoutes.MyPictures)
                    return IconName.MyPictures;
                if (route == SpecialRoutes.MyVideos)
                    return IconName.MyVideos;
                if (route == SpecialRoutes.Settings)
                    return IconName.Settings;
            }

            try
            {
                var attr = File.GetAttributes(route.FullName);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    var dirInfo = new DirectoryInfo(route.FullName);

                    if (dirInfo.Parent == null)
                    {
                        if (dirInfo.FullName == "C:\\")
                            return IconName.SystemDrive;

                        return IconName.LogicalDrive;
                    }

                    return IconName.Folder;
                }

                return GetExtensionKey(route.FullName);
            }
            catch (Exception e)
            {
                return IconName.Blank;
            }
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