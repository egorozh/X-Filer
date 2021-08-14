using System;
using System.IO;
using System.Windows;

namespace XFiler.SDK
{
    public static class Extensions
    {
        public static IXFilerApp XFilerApp(this Application application)
        {
            return (IXFilerApp)application;
        }

        public static FileSystemInfo? ToInfo(this string path)
        {
            try
            {
                var attr = File.GetAttributes(path);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    var info = new DirectoryInfo(path);

                    return info;
                }

                return new FileInfo(path);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DirectoryInfo GetRootName(this FileSystemInfo info) => info switch
        {
            DirectoryInfo directoryInfo => directoryInfo.Root,
            FileInfo fileInfo => fileInfo.Directory.Root,
            _ => throw new NotImplementedException(nameof(GetRootName))
        };
    }
}