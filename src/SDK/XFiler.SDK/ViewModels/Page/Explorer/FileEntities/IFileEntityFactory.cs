using System.IO;
using System.Threading.Tasks;

namespace XFiler.SDK;

public interface IFileEntityFactory
{
    Task<IFileSystemModel> CreateDirectory(DirectoryInfo directoryInfo, IFilesGroup filesGroup, IconSize iconSize);
    Task<IFileSystemModel> CreateFile(FileInfo fileInfo, IFilesGroup filesGroup, IconSize iconSize);
}