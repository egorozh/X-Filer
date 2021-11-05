using System;
using System.IO;

namespace XFiler.SDK;

public interface IFilesPresenter : IDirectoryModel, IFileSystemModel
{
    void Init(DirectoryInfo directoryInfo, IFilesGroup filesGroup, IFilesSorting filesSorting);

    event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;
}