using GongSolutions.Wpf.DragDrop;

namespace XFiler.SDK
{
    public class FilesPresenterFactory : IFilesPresenterFactory
    {
        private readonly IFileEntityFactory _fileEntityFactory;
        private readonly IDropTarget _dropTarget;
        private readonly IDragSource _dragSource;
        private readonly IWindowFactory _windowFactory;

        public FilesPresenterFactory(IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory)
        {
            _fileEntityFactory = fileEntityFactory;
            _dropTarget = dropTarget;
            _dragSource = dragSource;
            _windowFactory = windowFactory;
        }

        public IFilesPresenter CreatePresenter(PresenterType presenterType, string currentDirectory)
            => presenterType switch
            {
                PresenterType.Grid => new GridFilesPresenterViewModel(currentDirectory, _fileEntityFactory,
                    _dropTarget, _dragSource, _windowFactory),

                _ => new TileFilesPresenterViewModel(currentDirectory, _fileEntityFactory, _dropTarget, _dragSource,
                    _windowFactory)
            };
    }
}