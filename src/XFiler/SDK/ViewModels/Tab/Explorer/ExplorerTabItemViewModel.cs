using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace XFiler.SDK
{
    public class ExplorerTabItemViewModel : TabItemViewModel, IExplorerTabItemViewModel
    {
        #region Private Fields

        private readonly IDirectoryHistory _history;
        private string _searchText;

        #endregion

        #region Public Properties

        public IReadOnlyList<IFilesPresenterFactory> FilesPresenters { get; }
        public IFilesPresenterFactory CurrentPresenter { get; set; }

        public string SearchText
        {
            get => _searchText;
            set => SetSearchText(value);
        }

        public string CurrentDirectoryFileName => _history.Current.DirectoryPath;

        #endregion

        #region Commands

        public DelegateCommand<string> AddBookmarkCommand { get; }

        public DelegateCommand MoveBackCommand { get; }

        public DelegateCommand MoveForwardCommand { get; }

        #endregion

        #region Constructor

        public ExplorerTabItemViewModel(
            IReadOnlyList<IFilesPresenterFactory> filesPresenters,
            IBookmarksManager bookmarksManager,
            string directoryPath,
            string directoryName) : base(directoryName, CreateTemplate())
        {
            FilesPresenters = filesPresenters;

            AddBookmarkCommand = bookmarksManager.AddBookmarkCommand;

            _history = new DirectoryHistory(directoryPath, directoryName);

            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

            Header = _history.Current.DirectoryPathName;
            _searchText = _history.Current.DirectoryPath;

            _history.HistoryChanged += History_HistoryChanged;
            PropertyChanged += DirectoryTabItemViewModelOnPropertyChanged;

            foreach (var factory in filesPresenters)
                factory.DirectoryOrFileOpened += FilePresenterOnDirectoryOrFileOpened;

            CurrentPresenter = FilesPresenters.First();
        }


        public ExplorerTabItemViewModel(
            IReadOnlyList<IFilesPresenterFactory> filesPresenters,
            IBookmarksManager bookmarksManager,
            DirectoryInfo directoryInfo)
            : this(filesPresenters, bookmarksManager, directoryInfo.FullName, directoryInfo.Name)
        {
        }

        private void DirectoryTabItemViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged -= DirectoryTabItemViewModelOnPropertyChanged;

            switch (e.PropertyName)
            {
                case nameof(CurrentPresenter):

                    OpenDirectory();

                    break;
            }

            PropertyChanged += DirectoryTabItemViewModelOnPropertyChanged;
        }

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            base.Dispose();

            foreach (var factory in FilesPresenters)
                factory.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
        }

        public void OpenBookmark(string path)
        {
            var attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
                OpenDirectory(new DirectoryInfo(path));
            else
                OpenFile(path);
        }

        #endregion

        #region Commands Methods

        private void Open(FileEntityViewModel parameter)
        {
            switch (parameter)
            {
                case DirectoryViewModel directoryViewModel:
                    OpenDirectory(directoryViewModel.DirectoryInfo);
                    break;
                case FileViewModel fileViewModel:
                    OpenFile(fileViewModel.FullName);
                    break;
            }
        }

        private bool OnCanMoveForward() => _history.CanMoveForward;

        private void OnMoveForward()
        {
            _history.MoveForward();

            var current = _history.Current;

            SearchText = current.DirectoryPath;
            Header = current.DirectoryPathName;

            OpenDirectory();
        }

        private bool OnCanMoveBack() => _history.CanMoveBack;

        private void OnMoveBack()
        {
            _history.MoveBack();

            var current = _history.Current;

            SearchText = current.DirectoryPath;
            Header = current.DirectoryPathName;

            OpenDirectory();
        }

        #endregion

        #region Private Methods

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(ExplorerTabItemViewModel),
            VisualTree = new FrameworkElementFactory(typeof(ExplorerTabItem))
        };

        private void OpenDirectory(DirectoryInfo directoryInfo)
        {
            SearchText = directoryInfo.FullName;
            Header = directoryInfo.Name;

            _history.Add(SearchText, Header);

            OpenDirectory();
        }

        private static void OpenFile(string path) => new Process
        {
            StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            }
        }.Start();

        private void OpenDirectory()
        {
            CurrentPresenter.UpdatePresenter(CurrentDirectoryFileName);
        }

        private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
        {
            Open(e.FileEntityViewModel);
        }

        private void History_HistoryChanged(object? sender, EventArgs e)
        {
            MoveBackCommand?.RaiseCanExecuteChanged();
            MoveForwardCommand?.RaiseCanExecuteChanged();
        }

        private void SetSearchText(string text)
        {
            _searchText = text;
        }

        #endregion
    }
}