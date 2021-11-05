using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace XFiler.SDK;

public abstract class BaseFilesPresenter : DisposableViewModel, IFilesPresenter
{
    #region Private Fields

    private IFileEntityFactory _fileEntityFactory;
    private IReactiveOptions _settings;
    private IFileOperations _fileOperations;
    private ILogger _logger;

    private BackgroundWorker? _backgroundWorker;
    private FileSystemWatcher _watcher = null!;

    #endregion

    #region Public Properties

    public INaturalStringComparer NaturalStringComparer { get; private set; }
    public INativeContextMenuLoader NativeContextMenuLoader { get; private set; }

    public ObservableCollection<IFileSystemModel> DirectoriesAndFiles { get; set; } = new();

    public IDropTarget DropTarget { get; private set; }
    public IDragSource DragSource { get; private set; }

    public DirectoryInfo DirectoryInfo { get; private set; } = null!;

    public IFilesGroup Group { get; private set; } = null!;

    public IFilesSorting Sorting { get; private set; } = null!;

    public FileSystemInfo Info { get; private set; } = null!;

    public bool IsLoaded { get; set; }

    public int Progress { get; set; }

    public virtual IconSize IconSize { get; } = IconSize.Large;

    #endregion

    #region Events

    public event EventHandler<OpenDirectoryEventArgs>? DirectoryOrFileOpened;

    #endregion

    #region Commands

    public DelegateCommand<IFileSystemModel> OpenCommand { get; }
    public DelegateCommand<object> OpenNewTabCommand { get; private set; }
    public DelegateCommand<IFileSystemModel> OpenInNativeExplorerCommand { get; private set; }

    public DelegateCommand<object> PasteCommand { get; private set; }
    public DelegateCommand<object> CutCommand { get; private set; }
    public DelegateCommand<object> CopyCommand { get; private set; }

    public DelegateCommand<object> DeleteCommand { get; }
    public DelegateCommand<object> DeletePermanentlyCommand { get; }

    public DelegateCommand<object> OpenNewWindowCommand { get; private set; }

    public DelegateCommand<object> RenameCommand { get; private set; }
    public DelegateCommand<object> StartRenameCommand { get; private set; }

    #endregion

    #region Constructor

    protected BaseFilesPresenter(
        IFileEntityFactory fileEntityFactory,
        IDropTarget dropTarget,
        IDragSource dragSource,
        IWindowFactory windowFactory,
        IClipboardService clipboardService,
        IReactiveOptions settings,
        IFileOperations fileOperations,
        ILogger logger,
        IRenameService renameService,
        IMainCommands mainCommands,
        INaturalStringComparer naturalStringComparer,
        INativeContextMenuLoader nativeContextMenuLoader)
    {
        _fileEntityFactory = fileEntityFactory;
        _settings = settings;
        _fileOperations = fileOperations;
        _logger = logger;
        NaturalStringComparer = naturalStringComparer;
        NativeContextMenuLoader = nativeContextMenuLoader;

        DropTarget = dropTarget;
        DragSource = dragSource;

        OpenCommand = new DelegateCommand<IFileSystemModel>(Open);
        DeleteCommand = new DelegateCommand<object>(OnDelete);
        DeletePermanentlyCommand = new DelegateCommand<object>(OnPermanentlyDelete);


        OpenNewTabCommand = mainCommands.OpenNewTabCommand;
        OpenInNativeExplorerCommand = mainCommands.OpenInNativeExplorerCommand;
        OpenNewWindowCommand = windowFactory.OpenNewWindowCommand;

        PasteCommand = clipboardService.PasteCommand;
        CutCommand = clipboardService.CutCommand;
        CopyCommand = clipboardService.CopyCommand;

        RenameCommand = renameService.RenameCommand;
        StartRenameCommand = renameService.StartRenameCommand;
    }

    #endregion

    #region Public Methods

    public async void Init(DirectoryInfo directoryInfo, IFilesGroup @group, IFilesSorting filesSorting)
    {
        DirectoryInfo = directoryInfo;
        Info = directoryInfo;
        Group = group;
        Sorting = filesSorting;

        _watcher = new FileSystemWatcher(directoryInfo.FullName);

        _watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;

        await InitItems();

        _watcher.Changed += OnChanged;
        _watcher.Created += OnCreated;
        _watcher.Deleted += OnDeleted;
        _watcher.Renamed += OnRenamed;
        _watcher.Error += OnError;

        _watcher.IncludeSubdirectories = false;
        _watcher.EnableRaisingEvents = true;
    }


    public async Task InfoChanged(FileSystemInfo? newInfo)
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (!Disposed && disposing)
        {
            _watcher.Dispose();

            _watcher.Changed -= OnChanged;
            _watcher.Created -= OnCreated;
            _watcher.Deleted -= OnDeleted;
            _watcher.Renamed -= OnRenamed;
            _watcher.Error -= OnError;

            _watcher = null!;

            BeforeUpdateDispose();

            _fileEntityFactory = null!;
            _settings = null!;
            _fileOperations = null!;
            _logger = null!;
            NaturalStringComparer = null!;
            NativeContextMenuLoader = null!;

            Group = null!;
            Sorting = null!;
            DirectoryInfo = null!;
            Info = null!;

            DropTarget = null!;
            DragSource = null!;

            OpenNewTabCommand = null!;
            OpenNewWindowCommand = null!;
            OpenInNativeExplorerCommand = null!;

            PasteCommand = null!;
            CutCommand = null!;
            CopyCommand = null!;

            RenameCommand = null!;
            StartRenameCommand = null!;
        }

        base.Dispose(disposing);
    }

    #endregion

    #region Command Methods

    private void Open(IFileSystemModel fileEntityViewModel)
    {
        DirectoryOrFileOpened?.Invoke(this, new OpenDirectoryEventArgs(fileEntityViewModel));
    }

    private void OnDelete(object parameters)
    {
        switch (parameters)
        {
            case IFileSystemModel model:
                Delete(new[] {model.Info});
                break;
            case IEnumerable e:
                Delete(e.OfType<IFileSystemModel>().Select(m => m.Info));
                break;
        }
    }

    private void OnPermanentlyDelete(object parameters)
    {
        switch (parameters)
        {
            case IFileSystemModel model:
                Delete(new[] {model.Info}, true);
                break;
            case IEnumerable e:
                Delete(e.OfType<IFileSystemModel>().Select(m => m.Info), true);
                break;
        }
    }

    #endregion

    #region Private Methods

    private async Task InitItems()
    {
        IsLoaded = true;
        Progress = 0;

        try
        {
            var items = await GetItems();

            _backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;

            _backgroundWorker.RunWorkerAsync(items);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show("У вас нет доступа к данной папке", "Нет доступа");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private async Task<IFileSystemModel> CreateItem((FileSystemInfo, EntityType) item)
    {
        var (path, entityType) = item;

        return entityType switch
        {
            EntityType.Directory => await _fileEntityFactory.CreateDirectory((DirectoryInfo) path, Group, IconSize),
            EntityType.File => await _fileEntityFactory.CreateFile((FileInfo) path, Group, IconSize),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private async ValueTask<IReadOnlyList<(FileSystemInfo, EntityType)>> GetItems()
        => await Task.Run(() =>
        {
            List<(FileSystemInfo, EntityType)> list = new();

            if (Disposed)
                return list;

            var hideSystemFiles = !_settings.ShowSystemFiles;
            var hideHiddenFiles = !_settings.ShowHiddenFiles;
            
            var dirs = DirectoryInfo.EnumerateDirectories()
                .Where(f => NotSystemFilter(f, hideSystemFiles))
                .Where(f => NotHidenFilter(f, hideHiddenFiles));

            list.AddRange(Sorting.OrderBy(dirs)
                .Select(d => ((FileSystemInfo) d, EntityType.Directory)));

            var files = DirectoryInfo.EnumerateFiles()
                .Where(f => NotSystemFilter(f, hideSystemFiles))
                .Where(f => NotHidenFilter(f, hideHiddenFiles));

            list.AddRange(Sorting.OrderBy(files)
                .Select(d => ((FileSystemInfo) d, EntityType.File)));

            return list;
        });

    private static bool NotHidenFilter(FileSystemInfo info, bool hideHiddenFiles)
        => !hideHiddenFiles || !info.Attributes.HasFlag(FileAttributes.Hidden);

    private static bool NotSystemFilter(FileSystemInfo info, bool hideSystemFiles)
        => !hideSystemFiles || !info.Attributes.HasFlag(FileAttributes.System);

    private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        => Progress = e.ProgressPercentage;

    private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        if (e.Argument is not IReadOnlyList<(FileSystemInfo, EntityType)> items)
            return;

        var bw = (BackgroundWorker) sender!;

        var count = items.Count;

        for (var i = 0; i < count; i++)
        {
            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            bw.ReportProgress((int) (i / (double) count * 100.0));

            Application.Current.Dispatcher.Invoke(async () => { DirectoriesAndFiles.Add(await CreateItem(items[i])); });
        }
    }

    private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        IsLoaded = false;
    }

    private void Delete(IEnumerable<FileSystemInfo> items, bool isPermanently = false)
    {
        _fileOperations.Delete(items.ToList(), DirectoryInfo, isPermanently);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        var deletedItem = DirectoriesAndFiles
            .FirstOrDefault(vm => vm.Info.FullName == e.FullPath);

        if (deletedItem != null)
        {
            Application.Current.Dispatcher.Invoke(() => { DirectoriesAndFiles.Remove(deletedItem); });
        }
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        var info = e.FullPath.ToInfo();

        switch (info)
        {
            case DirectoryInfo directoryInfo:
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    DirectoriesAndFiles.Add(
                        await _fileEntityFactory.CreateDirectory(directoryInfo, Group, IconSize));
                });
                break;
            case FileInfo fileInfo:
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    DirectoriesAndFiles.Add(await _fileEntityFactory.CreateFile(fileInfo, Group, IconSize));
                });
                break;
        }
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        _logger.Error(e.GetException(), "File Watcher Error in directory {0}", Info.FullName);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        var renamedItem = DirectoriesAndFiles
            .FirstOrDefault(vm => vm.Info.FullName == e.OldFullPath);

        if (renamedItem != null)
        {
            Application.Current.Dispatcher.Invoke(async () => { await renamedItem.InfoChanged(e.FullPath.ToInfo()); });
        }
    }

    private void BeforeUpdateDispose()
    {
        if (_backgroundWorker != null)
        {
            _backgroundWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork -= BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged -= BackgroundWorker_ProgressChanged;

            if (_backgroundWorker is {IsBusy: true})
                _backgroundWorker.CancelAsync();


            _backgroundWorker.Dispose();
        }

        _backgroundWorker = null;

        foreach (var model in DirectoriesAndFiles)
            model.Dispose();
    }

    #endregion

    private enum EntityType
    {
        Directory,
        File
    }
}