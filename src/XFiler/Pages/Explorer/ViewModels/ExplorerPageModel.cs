using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler
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

        public DelegateCommand<object> PasteCommand { get; private set; }
            
        #region Constructor

        public ExplorerPageModel(
            IReadOnlyList<IFilesPresenterFactory> filesPresenters,
            IClipboardService clipboardService,
            DirectoryInfo directory) : base(typeof(ExplorerPage))
        {
            _directory = directory;

            FilesPresenters = filesPresenters;
            PasteCommand = clipboardService.PasteCommand;

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
            PasteCommand = null!;
        }

        #endregion

        #region Private Methods

        private void OpenDirectory()
        {
            CurrentPresenter.UpdatePresenter(_directory);
        }

        private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
        {
            XFilerRoute route = e.FileEntityViewModel switch
            {
                DirectoryViewModel directoryViewModel => directoryViewModel.Route,
                FileViewModel fileViewModel => fileViewModel.Route,
                _ => SpecialRoutes.MyComputer
            };

            GoTo(route);
        }

        #endregion
    }
}