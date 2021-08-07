using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace XFiler.SDK
{
    public class ExplorerPageModel : BasePageModel, IExplorerPageModel
    {
        #region Private Fields

        private DirectoryInfo _directory;

        #endregion

        #region Public Properties

        public IReadOnlyList<IFilesPresenterFactory> FilesPresenters { get; private set; }
        public IFilesPresenterFactory CurrentPresenter { get; set; }

        #endregion

        #region Constructor

        public ExplorerPageModel(
            IReadOnlyList<IFilesPresenterFactory> filesPresenters,
            DirectoryInfo directory) : base(CreateTemplate())
        {
            _directory = directory;

            FilesPresenters = filesPresenters;

            PropertyChanged += DirectoryTabItemViewModelOnPropertyChanged;

            foreach (var factory in filesPresenters)
                factory.DirectoryOrFileOpened += FilePresenterOnDirectoryOrFileOpened;

            CurrentPresenter = FilesPresenters.First();
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

            PropertyChanged -= DirectoryTabItemViewModelOnPropertyChanged;

            foreach (var factory in FilesPresenters)
                factory.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;

            _directory = null!;
            FilesPresenters = null!;
            CurrentPresenter = null!;
        }

        #endregion

        #region Private Methods

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(ExplorerPageModel),
            VisualTree = new FrameworkElementFactory(typeof(ExplorerPage))
        };

        private void OpenDirectory()
        {
            CurrentPresenter.UpdatePresenter(_directory);
        }

        private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
        {
            GoTo(new XFilerUrl(e.FileEntityViewModel.Name, e.FileEntityViewModel.FullName));
        }

        #endregion
    }
}