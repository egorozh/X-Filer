using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace XFiler.SDK;

public interface IFilesPresenterFactory : INotifyPropertyChanged, IDisposable, ICheckedItem
{
    IFilesPresenter? FilesPresenter { get; }
        
    DataTemplate Template { get; }
        
    string Id { get; }

    event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;

    void UpdatePresenter(DirectoryInfo directory, IFilesGroup currentGroup, IFilesSorting currentSorting);
}