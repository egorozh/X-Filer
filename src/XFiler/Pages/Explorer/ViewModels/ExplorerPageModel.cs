using System.ComponentModel;
using System.IO;

namespace XFiler;

public sealed class ExplorerPageModel : BasePageModel, IExplorerPageModel
{
    #region Private Fields

    private IDirectorySettings _directorySettings;
    private DirectoryInfo _directory;

    #endregion

    #region Public Properties

    public IReadOnlyList<IFilesPresenterFactory> FilesPresenters { get; private set; }
    public IFilesPresenterFactory CurrentPresenter { get; set; }

    public IReadOnlyList<IFilesGroup> FilesGroups { get; private set; }
    public IFilesGroup CurrentGroup { get; set; }

    #endregion

    public DelegateCommand<object> PasteCommand { get; private set; }
    public DelegateCommand<IFileSystemModel> CreateFolderCommand { get; private set; }
    public DelegateCommand<IFileSystemModel> CreateTextCommand { get; private set; }
    public DelegateCommand<IFileSystemModel> OpenInNativeExplorerCommand { get; private set; }

    #region Constructor

    public ExplorerPageModel(
        IReadOnlyList<IFilesPresenterFactory> filesPresenters,
        IReadOnlyList<IFilesGroup> groups,
        IClipboardService clipboardService,
        IMainCommands mainCommands,
        IDirectorySettings directorySettings,
        IReactiveOptions reactiveOptions,
        DirectoryInfo directory) : base(typeof(ExplorerPage), new DirectoryRoute(directory))
    {
        _directorySettings = directorySettings;
        _directory = directory;

        FilesPresenters = filesPresenters;
        FilesGroups = groups;
        PasteCommand = clipboardService.PasteCommand;

        CreateFolderCommand = mainCommands.CreateFolderCommand;
        CreateTextCommand = mainCommands.CreateTextCommand;
        OpenInNativeExplorerCommand = mainCommands.OpenInNativeExplorerCommand;

        var dirSettings = directorySettings.GetSettings(directory.FullName);

        CurrentGroup = FilesGroups.FirstOrDefault(g => g.Id == dirSettings.GroupId) ??
                       FilesGroups.First();

        PropertyChanged += DirectoryTabItemViewModelOnPropertyChanged;

        foreach (var factory in filesPresenters)
            factory.DirectoryOrFileOpened += FilePresenterOnDirectoryOrFileOpened;

        CurrentPresenter = SelectInitPresenter(dirSettings, reactiveOptions);
    }

    #endregion

    #region Public Methods

    public override void Dispose()
    {
        base.Dispose();

        PropertyChanged -= DirectoryTabItemViewModelOnPropertyChanged;

        foreach (var factory in FilesPresenters)
        {
            factory.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;

            factory.Dispose();
        }

        _directory = null!;
        _directorySettings = null!;
        FilesPresenters = null!;
        CurrentPresenter = null!;
        FilesGroups = null!;
        CurrentGroup = null!;

        PasteCommand = null!;
        CreateFolderCommand = null!;
        CreateTextCommand = null!;
        OpenInNativeExplorerCommand = null!;
    }

    #endregion

    #region Private Methods

    private void OpenDirectory()
    {
        CurrentPresenter.UpdatePresenter(_directory, CurrentGroup);
    }

    private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
    {
        XFilerRoute route = e.FileEntityViewModel switch
        {
            DirectoryViewModel directoryViewModel => new DirectoryRoute(directoryViewModel.DirectoryInfo),
            FileViewModel fileViewModel => new FileRoute(fileViewModel.FileInfo),
            _ => SpecialRoutes.MyComputer
        };

        GoTo(route);
    }

    private void DirectoryTabItemViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged -= DirectoryTabItemViewModelOnPropertyChanged;

        switch (e.PropertyName)
        {
            case nameof(CurrentPresenter):
            case nameof(CurrentGroup):
                OpenDirectory();
                _directorySettings.SetSettings(_directory.FullName,
                    new DirectorySettingsInfo(CurrentGroup.Id, CurrentPresenter.Id));
                break;
        }

        PropertyChanged += DirectoryTabItemViewModelOnPropertyChanged;
    }

    private IFilesPresenterFactory SelectInitPresenter(DirectorySettingsInfo dirSettings,
        IReactiveOptions options)
    {
        var presenterId = options.DefaultPresenterId;

        if (!options.AlwaysOpenDirectoryInDefaultPresenter) 
            presenterId = dirSettings.PresenterId ?? presenterId;

        return FilesPresenters.FirstOrDefault(p => p.Id == presenterId) ??
               FilesPresenters.First();
    }

    #endregion
}