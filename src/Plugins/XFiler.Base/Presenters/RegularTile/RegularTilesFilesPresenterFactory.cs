using GongSolutions.Wpf.DragDrop;
using System.Windows;
using XFiler.SDK;

namespace XFiler.Base
{
    public class RegularTilesFilesPresenterFactory : BaseFilesPresenterFactory
    {
        private readonly IFileEntityFactory _fileEntityFactory;
        private readonly IDropTarget _dropTarget;
        private readonly IDragSource _dragSource;
        private readonly IWindowFactory _windowFactory;

        public RegularTilesFilesPresenterFactory(IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory) : base("Крупные значки", CreateTemplate())
        {
            _fileEntityFactory = fileEntityFactory;
            _dropTarget = dropTarget;
            _dragSource = dragSource;
            _windowFactory = windowFactory;
        }

        public override IFilesPresenter CreatePresenter(string currentDirectory)
            => new TileFilesPresenterViewModel(currentDirectory, _fileEntityFactory, _dropTarget, _dragSource,
                _windowFactory);

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(TileFilesPresenterViewModel),
            VisualTree = new FrameworkElementFactory(typeof(TileFilePresenter))
        };
    }
}