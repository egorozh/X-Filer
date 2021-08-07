using System;
using System.IO;

namespace XFiler.SDK
{
    public sealed record XFilerUrl
    {
        public string Header { get; }

        public string FullName { get; }

        public XFilerUrl(string header, string fullName)
        {
            Header = header;
            FullName = fullName;
        }

        public XFilerUrl(FileSystemInfo directory)
        {
            Header = directory.Name;
            FullName = directory.FullName;
        }

        public static XFilerUrl FromPath(string path)
        {
            var special = SpecialUrls.GetSpecialUrl(path);

            if (special != null)
                return special;

            try
            {
                var attr = File.GetAttributes(path);

                FileSystemInfo file = attr.HasFlag(FileAttributes.Directory)
                    ? new DirectoryInfo(path)
                    : new FileInfo(path);

                return new XFilerUrl(file);
            }
            catch (Exception e)
            {
            }

            return SpecialUrls.MyComputer;
        }
    }
}