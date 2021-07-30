using System;
using System.Collections.ObjectModel;
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

        #endregion

        #region Public Properties

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new();

        public FileEntityViewModel? SelectedFileEntity { get; set; }

        public IDropTarget DropTarget { get; }
        public IDragSource DragSource { get; }

        public string CurrentDirectoryPathName { get; }

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
            string directoryPathName)
        {
            _fileEntityFactory = fileEntityFactory;
            DropTarget = dropTarget;
            DragSource = dragSource;
            CurrentDirectoryPathName = directoryPathName;

            OpenNewWindowCommand = windowFactory.OpenNewWindowCommand;


            OpenCommand = new DelegateCommand<object>(Open);
            OpenNewTabCommand = new DelegateCommand<object>(OpenNewTab);

            LoadItems();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
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

        private async void LoadItems()
        {
            if (CurrentDirectoryPathName == IExplorerApp.RootName)
            {
                await LoadRootItems();

                return;
            }

            var directoryInfo = new DirectoryInfo(CurrentDirectoryPathName);

            var comparer = new NaturalSortComparer();

            try
            {
                var directories = directoryInfo.EnumerateDirectories()
                    .OrderBy(d => d.Name, comparer);

                foreach (var directory in directories)
                {
                    var item = _fileEntityFactory.CreateDirectory(directory);
                    
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(item);
                    });
                }

                var files = directoryInfo.EnumerateFiles()
                    .OrderBy(f => f.Name, comparer);

                foreach (var fileInfo in files)
                {
                    var item = _fileEntityFactory.CreateFile(fileInfo);

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(item);
                    });
                }
            }
            catch (Exception ex)
            {
                //TODO: Try Exception 
            }
        }

        private async Task LoadRootItems()
        {
            var group = "Папки";

            var specialFolders = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
            };

            foreach (var specialFolder in specialFolders)
            {
                var item = _fileEntityFactory.CreateDirectory(new DirectoryInfo(specialFolder), @group);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    DirectoriesAndFiles.Add(item);
                });
            }

            @group = "Устройства и диски";

            foreach (var logicalDrive in Directory.GetLogicalDrives())
            {
                var item = _fileEntityFactory.CreateLogicalDrive(logicalDrive, @group);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    DirectoriesAndFiles.Add(item);
                });
            }
        }

        #endregion
    }
}