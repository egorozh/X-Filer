using System;
using System.IO;

namespace XFiler.SDK
{
    public interface IFileSystemModel : IDisposable
    {
        FileSystemInfo Info { get; }

        void InfoChanged(FileSystemInfo? newInfo);
    }

    public interface IFileItem
    {
        string Name { get; }

        string Type { get; }
    }
}