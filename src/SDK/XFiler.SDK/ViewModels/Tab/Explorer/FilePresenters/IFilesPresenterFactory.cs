using System;
using System.ComponentModel;
using System.Windows;

namespace XFiler.SDK
{
    public interface IFilesPresenterFactory : INotifyPropertyChanged, IDisposable
    {
        IFilesPresenter? FilesPresenter { get; }

        string Name { get; }

        DataTemplate Template { get; }

        event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;

        void UpdatePresenter(string currentDirectory);
    }
}