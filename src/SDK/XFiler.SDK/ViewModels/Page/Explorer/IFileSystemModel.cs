using System;
using System.IO;
using System.Threading.Tasks;

namespace XFiler.SDK;

public interface IFileSystemModel : IDisposable
{
    FileSystemInfo Info { get; }

    Task InfoChanged(FileSystemInfo? newInfo);
}
        
public interface IFileItem
{
    string Name { get; }

    string Type { get; }
}