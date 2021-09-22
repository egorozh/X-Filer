using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace XFiler.SDK
{
    public abstract class BaseFilesPresenterFactory : BaseViewModel, IFilesPresenterFactory
    {
        public IFilesPresenter? FilesPresenter { get; private set; }

        public ImageSource IconSource { get; }

        public string Name { get; }

        public DataTemplate Template { get; }

        public string Id { get; }

        public event EventHandler<OpenDirectoryEventArgs>? DirectoryOrFileOpened;

        protected BaseFilesPresenterFactory(
            string name,
            DataTemplate template, 
            ImageSource iconSource, 
            string id)
        {
            Name = name;
            Template = template;
            IconSource = iconSource;
            Id = id;
        }

        public void UpdatePresenter(DirectoryInfo directory, IFilesGroup group)
        {
            if (FilesPresenter != null)
            {
                FilesPresenter.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
                FilesPresenter.Dispose();
            }

            FilesPresenter = CreatePresenter(directory, group);

            FilesPresenter.DirectoryOrFileOpened += FilePresenterOnDirectoryOrFileOpened;
        }

        public virtual void Dispose()
        {
            if (FilesPresenter != null)
            {
                FilesPresenter.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
                FilesPresenter.Dispose();
            }
        }

        public abstract IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup group);

        private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
        {
            DirectoryOrFileOpened?.Invoke(sender, e);
        }
    }
}