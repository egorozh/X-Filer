using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
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
    public abstract class BaseFilesPresenter : BaseViewModel, IFilesPresenter
    {
        #region Private Fields

        private IFileEntityFactory _fileEntityFactory;
        private BackgroundWorker? _backgroundWorker;

        #endregion

        #region Public Properties

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new();

        public IDropTarget DropTarget { get; private set; }
        public IDragSource DragSource { get; private set; }

        public DirectoryInfo DirectoryInfo { get; }

        public FileSystemInfo Info { get; }

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

        public DelegateCommand<object> OpenNewWindowCommand { get; private set; }

        #endregion

        #region Constructor

        protected BaseFilesPresenter(
            IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory,
            IClipboardService clipboardService,
            DirectoryInfo directoryPathName)
        {
            _fileEntityFactory = fileEntityFactory;
            DropTarget = dropTarget;
            DragSource = dragSource;
            DirectoryInfo = directoryPathName;
            Info = directoryPathName;

            OpenCommand = new DelegateCommand<FileEntityViewModel>(Open);
            OpenNewTabCommand = new DelegateCommand<object>(OpenNewTab);

            OpenNewWindowCommand = windowFactory.OpenNewWindowCommand;

            PasteCommand = clipboardService.PasteCommand;
            CutCommand = clipboardService.CutCommand;
            CopyCommand = clipboardService.CopyCommand;

            InitItems();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (_backgroundWorker is { IsBusy: true })
                _backgroundWorker.CancelAsync();

            _backgroundWorker?.Dispose();
            _backgroundWorker = null;

            foreach (var model in DirectoriesAndFiles)
                model.Dispose();

            _fileEntityFactory = null!;

            DropTarget = null!;
            DragSource = null!;

            OpenNewWindowCommand = null!;

            PasteCommand = null!;
            CutCommand = null!;
            CopyCommand = null!;
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

            var comparer = new NaturalSortComparer();

            var hideSystemFiles = false;

            list.AddRange(DirectoryInfo.EnumerateDirectories()
                .Where(f => NotHidenFilter(f, hideSystemFiles))
                .OrderBy(d => d.Name, comparer)
                .Select(d => ((FileSystemInfo)d, EntityType.Directory)));

            list.AddRange(DirectoryInfo.EnumerateFiles()
                .Where(f => NotHidenFilter(f, hideSystemFiles))
                .OrderBy(d => d.Name, comparer)
                .Select(d => ((FileSystemInfo)d, EntityType.File)));

            return list;
        });

        private static bool NotHidenFilter(FileSystemInfo info, bool hideSystemFiles)
            => !hideSystemFiles || !info.Attributes.HasFlag(FileAttributes.Hidden);

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

        #endregion

        private enum EntityType
        {
            Directory,
            File
        }
    }
}