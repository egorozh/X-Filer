using GongSolutions.Wpf.DragDrop;

namespace XFiler.SDK
{
    public class TileFilesPresenterViewModel : BaseFilesPresenter
    {
        public TileFilesPresenterViewModel(string directoryPathName,
            IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory) :
            base(fileEntityFactory, dropTarget, dragSource, windowFactory, directoryPathName)
        {
        }
    }
}