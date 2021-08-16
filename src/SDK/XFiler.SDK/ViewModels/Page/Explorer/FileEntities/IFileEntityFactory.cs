using System.IO;

namespace XFiler.SDK
{
    public interface IFileEntityFactory
    {
        FileEntityViewModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null);
        FileEntityViewModel CreateFile(FileInfo fileInfo, string? @group = null);
    }
}