using System;
using System.IO;
using System.Windows;

namespace XFiler.SDK
{
    public abstract class BaseFilesPresenterFactory : BaseViewModel, IFilesPresenterFactory
    {
        public IFilesPresenter? FilesPresenter { get; private set; }

        public string Name { get; }

        public DataTemplate Template { get; }

        public event EventHandler<OpenDirectoryEventArgs>? DirectoryOrFileOpened;

        protected BaseFilesPresenterFactory(string name, DataTemplate template)
        {
            Name = name;
            Template = template;
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