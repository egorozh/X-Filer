using System;

namespace XFiler.SDK
{
    public interface IFilesPresenter : IDisposable, IDirectoryModel, IFileSystemModel
    {
        event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;
    }
}