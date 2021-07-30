using System;

namespace XFiler.SDK
{
    public interface IFilesPresenter : IDisposable
    {
        string CurrentDirectoryPathName { get; }

        event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;
    }
}