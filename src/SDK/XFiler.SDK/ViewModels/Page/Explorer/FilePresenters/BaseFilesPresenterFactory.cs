using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace XFiler.SDK;

public abstract class BaseFilesPresenterFactory : BaseViewModel, IFilesPresenterFactory
{
    public IFilesPresenter? FilesPresenter { get; private set; }

    public ImageSource IconSource { get; private set; }

    public string Name { get; }

    public DataTemplate Template { get; private set; }

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

    public void UpdatePresenter(DirectoryInfo directory, IFilesGroup @group, IFilesSorting sorting)
    {
        if (FilesPresenter != null)
        {
            FilesPresenter.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
            FilesPresenter.Dispose();
        }

        FilesPresenter = CreatePresenter(directory, group, sorting);

        FilesPresenter.DirectoryOrFileOpened += FilePresenterOnDirectoryOrFileOpened;
    }

    public virtual void Dispose()
    {
        if (FilesPresenter != null)
        {
            FilesPresenter.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
            FilesPresenter.Dispose();
        }

        IconSource = null!;
        Template = null!;
    }

    public abstract IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup @group,
        IFilesSorting filesSorting);

    private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
    {
        DirectoryOrFileOpened?.Invoke(sender, e);
    }
}