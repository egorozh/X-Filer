using System.IO;

namespace XFiler.SDK
{
    public interface IFileEntityFactory
    {
        IFileSystemModel CreateDirectory(DirectoryInfo directoryInfo, IFilesGroup filesGroup);
        IFileSystemModel CreateFile(FileInfo fileInfo, IFilesGroup filesGroup);
    }
}