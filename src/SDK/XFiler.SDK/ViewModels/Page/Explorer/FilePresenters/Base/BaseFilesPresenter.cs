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

namespace XFiler.SDK
{
    public abstract class BaseFilesPresenter : DisposableViewModel, IFilesPresenter
    {
        #region Private Fields

        private IFileEntityFactory _fileEntityFactory;
        private IExplorerOptions _settings;
        private IFileOperations _fileOperations;
        private ILogger _logger;
        private BackgroundWorker? _backgroundWorker;
        private FileSystemWatcher _watcher = null!;

        #endregion

        #region Public Properties

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new();

        public IDropTarget DropTarget { get; private set; }
        public IDragSource DragSource { get; private set; }

        public DirectoryInfo DirectoryInfo { get; private set; } = null!;

        public FileSystemInfo Info { get; private set; } = null!;

        public bool IsLoaded { get; set; }

        public int Progress { get; set; }

        #endregion

        #region Events

        public event EventHandler<OpenDirectoryEventArgs>? DirectoryOrFileOpened;

        #endregion

        #region Commands

        public DelegateCommand<FileEntityViewModel> OpenCommand { get; }
        public DelegateCommand<object> OpenNewTabCommand { get; }
        
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
            IExplorerOptions settings,
            IFileOperations fileOperations,
            ILogger logger,
            IRenameService renameService)
        {
            _fileEntityFactory = fileEntityFactory;
            _settings = settings;
            _fileOperations = fileOperations;
            _logger = logger;

            DropTarget = dropTarget;
            DragSource = dragSource;

            OpenCommand = new DelegateCommand<FileEntityViewModel>(Open);
            DeleteCommand = new DelegateCommand<object>(OnDelete);
            DeletePermanentlyCommand = new DelegateCommand<object>(OnPermanentlyDelete);
            

            OpenNewTabCommand = new DelegateCommand<object>(OpenNewTab);

            OpenNewWindowCommand = windowFactory.OpenNewWindowCommand;

            PasteCommand = clipboardService.PasteCommand;
            CutCommand = clipboardService.CutCommand;
            CopyCommand = clipboardService.CopyCommand;

            RenameCommand = renameService.RenameCommand;
            StartRenameCommand = renameService.StartRenameCommand;
        }
        
        #endregion

        #region Public Methods

        public void Init(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
            Info = directoryInfo;

            _watcher = new FileSystemWatcher(directoryInfo.FullName);

            _watcher.NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size;

            InitItems();

            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
            _watcher.Error += OnError;

            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
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

                DropTarget = null!;
                DragSource = null!;

                OpenNewWindowCommand = null!;

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

        private void Open(FileEntityViewModel fileEntityViewModel)
        {
            DirectoryOrFileOpened?.Invoke(this, new OpenDirectoryEventArgs(fileEntityViewModel));
        }

        private void OpenNewTab(object parameter)
        {
            if (parameter is object[] { Length: 2 } parameters &&
                parameters[0] is ITabsViewModel tabsModel)
            {
                switch (parameters[1])
                {
                    case FileEntityViewModel fileEntityViewModel:
                        tabsModel.OnOpenNewTab(fileEntityViewModel);
                        break;
                    case IEnumerable e:
                        tabsModel.OnOpenNewTab(e.OfType<IFileSystemModel>());
                        break;
                }
            }
        }

        private void OnDelete(object parameters)
        {
            switch (parameters)
            {
                case IFileSystemModel model:
                    Delete(new[] { model.Info });
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
                    Delete(new[] { model.Info }, true);
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
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("У вас нет доступа к данной папке", "Нет доступа");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private FileEntityViewModel CreateItem((FileSystemInfo, EntityType) item)
        {
            var (path, entityType) = item;

            return entityType switch
            {
                EntityType.Directory => _fileEntityFactory.CreateDirectory((DirectoryInfo)path),
                EntityType.File => _fileEntityFactory.CreateFile((FileInfo)path),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private async Task<IReadOnlyList<(FileSystemInfo, EntityType)>> GetItems() => await Task.Run(() =>
        {
            List<(FileSystemInfo, EntityType)> list = new();

            if (Disposed)
                return list;

            var comparer = new NaturalSortComparer();

            var hideSystemFiles = !_settings.ShowSystemFiles;
            var hideHiddenFiles = !_settings.ShowHiddenFiles;

            list.AddRange(DirectoryInfo.EnumerateDirectories()
                .Where(f => NotSystemFilter(f, hideSystemFiles))
                .Where(f => NotHidenFilter(f, hideHiddenFiles))
                .OrderBy(d => d.Name, comparer)
                .Select(d => ((FileSystemInfo)d, EntityType.Directory)));

            list.AddRange(DirectoryInfo.EnumerateFiles()
                .Where(f => NotSystemFilter(f, hideSystemFiles))
                .Where(f => NotHidenFilter(f, hideHiddenFiles))
                .OrderBy(d => d.Name, comparer)
                .Select(d => ((FileSystemInfo)d, EntityType.File)));

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

            var bw = (BackgroundWorker)sender!;

            var count = items.Count;

            for (var i = 0; i < count; i++)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                bw.ReportProgress((int)(i / (double)count * 100.0));

                Application.Current.Dispatcher.Invoke(() => { DirectoriesAndFiles.Add(CreateItem(items[i])); });
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
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DirectoriesAndFiles.Add(_fileEntityFactory.CreateDirectory(directoryInfo));
                    });
                    break;
                case FileInfo fileInfo:
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DirectoriesAndFiles.Add(_fileEntityFactory.CreateFile(fileInfo));
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
        }

        private void BeforeUpdateDispose()
        {
            if (_backgroundWorker != null)
            {
                _backgroundWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
                _backgroundWorker.DoWork -= BackgroundWorker_DoWork;
                _backgroundWorker.ProgressChanged -= BackgroundWorker_ProgressChanged;

                if (_backgroundWorker is { IsBusy: true })
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
}