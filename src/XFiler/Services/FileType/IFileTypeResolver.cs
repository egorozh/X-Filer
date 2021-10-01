using System.IO;

namespace XFiler;

public interface IFileTypeResolver
{
    string GetFileType(FileSystemInfo info);
}