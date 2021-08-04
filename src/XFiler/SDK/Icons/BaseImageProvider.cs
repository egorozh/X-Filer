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

        public ImageSource? GetIcon(IMenuItemViewModel viewModel, double size)
        {
            var key = GetResourceKey(viewModel);

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

        private static string GetResourceKey(IMenuItemViewModel menuItemViewModel)
        {
            var path = menuItemViewModel.Path;

            if (path == null)
                return IconName.BookmarkFolder;

            var attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                var dirInfo = new DirectoryInfo(path);

                if (dirInfo.Parent == null)
                {
                    if (dirInfo.FullName == "C:\\")
                        return IconName.SystemDrive;

                    return IconName.LogicalDrive;
                }

                return IconName.Folder;
            }

            return GetExtensionKey(path);
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
        }
    }
}