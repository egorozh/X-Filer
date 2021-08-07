using System;
using System.IO;

namespace XFiler.SDK
{
    public interface IFilesPresenter : IDisposable
    {
        DirectoryInfo CurrentDirectory { get; }

        event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;
    }
}