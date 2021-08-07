using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;

namespace XFiler.SDK
{
    public abstract class BaseFilesPresenter : BaseViewModel, IFilesPresenter
    {
        #region Private Fields

        private readonly IFileEntityFactory _fileEntityFactory;
        private BackgroundWorker? _backgroundWorker;

        #endregion

        #region Public Properties

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new();

        public IDropTarget DropTarget { get; }
        public IDragSource DragSource { get; }

        public DirectoryInfo CurrentDirectory { get; }

        public bool IsLoaded { get; set; }

        public int Progress { get; set; }

        #endregion

        #region Events

        public event EventHandler<OpenDirectoryEventArgs>? DirectoryOrFileOpened;

        #endregion

        #region Commands

        public DelegateCommand<object> OpenCommand { get; }
        public DelegateCommand<object> OpenNewTabCommand { get; }

        public DelegateCommand<FileEntityViewModel> OpenNewWindowCommand { get; }

        #endregion

        #region Constructor

        protected BaseFilesPresenter(
            IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory,
            DirectoryInfo directoryPathName)
        {
            _fileEntityFactory = fileEntityFactory;
            DropTarget = dropTarget;
            DragSource = dragSource;
            CurrentDirectory = directoryPathName;

            OpenNewWindowCommand = windowFactory.OpenNewWindowCommand;


            OpenCommand = new DelegateCommand<object>(Open);
            OpenNewTabCommand = new DelegateCommand<object>(OpenNewTab);

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
        }

        #endregion

        #region Command Methods

        private void Open(object parameter)
        {
            if (parameter is FileEntityViewModel fileEntityViewModel)
                DirectoryOrFileOpened?.Invoke(this, new OpenDirectoryEventArgs(fileEntityViewModel));
        }

        private void OpenNewTab(object parameter)
        {
            if (parameter is object[] { Length: 2 } parameters &&
                parameters[0] is ITabsViewModel mainViewModel &&
                parameters[1] is FileEntityViewModel fileEntityViewModel)
            {
                mainViewModel.OnOpenNewTab(fileEntityViewModel);
            }
        }

        #endregion

        #region Private Methods

        private async void InitItems()
        {
            IsLoaded = true;
            Progress = 0;

            try
            {
                IReadOnlyList<(object, EntityType)> items = await GetItems();

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


        private FileEntityViewModel CreateItem((object, EntityType) item)
        {
            var (path, entityType) = item;

            return entityType switch
            {
                EntityType.SpecialFolder =>
                    _fileEntityFactory.CreateDirectory(new DirectoryInfo((string)path), "Папки"),
                EntityType.Drive => _fileEntityFactory.CreateLogicalDrive((string)path, "Устройства и диски"),
                EntityType.Directory => _fileEntityFactory.CreateDirectory((DirectoryInfo)path),
                EntityType.File => _fileEntityFactory.CreateFile((FileInfo)path),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private async Task<IReadOnlyList<(object, EntityType)>> GetItems() => await Task.Run(() =>
        {
            List<(object, EntityType)> list = new();

            //if (CurrentDirectoryPathName == IXFilerApp.RootName)
            //{
            //    list.AddRange(new (object, EntityType)[]
            //    {
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            //            EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.Desktop), EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), EntityType.SpecialFolder),
            //        (Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), EntityType.SpecialFolder)
            //    });

            //    list.AddRange(Directory.GetLogicalDrives().Select(p => ((object)p, EntityType.Drive)));
            //}
            //else
            //{
           
            var comparer = new NaturalSortComparer();

            list.AddRange(CurrentDirectory.EnumerateDirectories()
                .OrderBy(d => d.Name, comparer)
                .Select(d => ((object)d, EntityType.Directory)));

            list.AddRange(CurrentDirectory.EnumerateFiles()
                .OrderBy(d => d.Name, comparer)
                .Select(d => ((object)d, EntityType.File)));
            //}

            return list;
        });

        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (e.Argument is not IReadOnlyList<(object, EntityType)> items)
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
            SpecialFolder,
            Drive,
            Directory,
            File
        }
    }
}