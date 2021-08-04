using System;
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

        public void UpdatePresenter(string currentDirectory)
        {
            if (FilesPresenter != null)
            {
                FilesPresenter.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
                FilesPresenter.Dispose();
            }

            FilesPresenter = CreatePresenter(currentDirectory);

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

        public abstract IFilesPresenter CreatePresenter(string currentDirectory);

        private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
        {
            DirectoryOrFileOpened?.Invoke(sender, e);
        }
    }
}