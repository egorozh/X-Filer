using System.IO;
using XFiler.Resources.Localization;

namespace XFiler;

internal class FileTypeResolver : IFileTypeResolver
{
    public string GetFileType(FileSystemInfo info)
    {   
        return info switch
        {
            DirectoryInfo directoryInfo => Strings.FileVm_DirectoryTypeName,
            FileInfo fileInfo => fileInfo.Extension[1..].ToUpper(),
            _ => throw new ArgumentOutOfRangeException(nameof(info))
        };
    }
}