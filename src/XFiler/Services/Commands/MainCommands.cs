using System.Diagnostics;

namespace XFiler.Commands;

internal sealed class MainCommands : IMainCommands
{
    private readonly IFileOperations _fileOperations;

    public DelegateCommand<IFileSystemModel> CreateFolderCommand { get; }
    public DelegateCommand<IFileSystemModel> CreateTextCommand { get; }
    public DelegateCommand<IFileSystemModel> OpenInNativeExplorerCommand { get; }
    public DelegateCommand<object> OpenNewTabCommand { get; }

    public MainCommands(IFileOperations fileOperations)
    {
        _fileOperations = fileOperations;
        CreateFolderCommand = new DelegateCommand<IFileSystemModel>(OnCreateFolder);
        CreateTextCommand = new DelegateCommand<IFileSystemModel>(OnCreateText);
        OpenInNativeExplorerCommand = new DelegateCommand<IFileSystemModel>(OnOpenInNativeExplorer);
        OpenNewTabCommand = new DelegateCommand<object>(OpenNewTab);
    }

    private void OnCreateFolder(IFileSystemModel targetDir)
    {
        _fileOperations.CreateFolder(targetDir.Info.FullName);
    }

    private void OnCreateText(IFileSystemModel targetDir)
    {
        _fileOperations.CreateEmptyTextFile(targetDir.Info.FullName);
    }

    private static void OnOpenInNativeExplorer(IFileSystemModel openedDir) => new Process
    {
        StartInfo =
        {
            FileName = "explorer",
            Arguments = openedDir.Info.FullName
        }
    }.Start();

    private static void OpenNewTab(object parameter)
    {
        if (parameter is object[] { Length: 2 } parameters &&
            parameters[0] is ITabsViewModel tabsModel)
        {
            switch (parameters[1])
            {
                case IFileSystemModel fileEntityViewModel:
                    tabsModel.OnOpenNewTab(fileEntityViewModel);
                    break;
                case IEnumerable e:
                    tabsModel.OnOpenNewTab(e.OfType<IFileSystemModel>());
                    break;
            }
        }
    }
}