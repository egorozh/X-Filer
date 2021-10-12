using Prism.Commands;

namespace XFiler.SDK;

public interface IMainCommands
{
    DelegateCommand<IFileSystemModel> CreateFolderCommand { get; }
    DelegateCommand<IFileSystemModel> CreateTextCommand { get; }

    DelegateCommand<IFileSystemModel> OpenInNativeExplorerCommand { get; }

    DelegateCommand<object> OpenNewTabCommand { get; }
}