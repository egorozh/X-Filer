using System.IO;

namespace XFiler.SDK
{
    public interface IFileEntityFactory
    {
        IFileSystemModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null);
        IFileSystemModel CreateFile(FileInfo fileInfo, string? @group = null);
    }
}