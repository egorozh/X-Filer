using System;
using System.IO;

namespace XFiler.SDK
{
    public interface IFilesPresenter : IDirectoryModel, IFileSystemModel
    {
        void Init(DirectoryInfo directoryInfo, IFilesGroup filesGroup);

        event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;
    }
}