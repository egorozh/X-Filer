using System.IO;

namespace XFiler.SDK
{
    public interface IFileEntityFactory
    {
        DirectoryViewModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null);
        FileViewModel CreateFile(FileInfo fileInfo, string? @group = null);
    }
}