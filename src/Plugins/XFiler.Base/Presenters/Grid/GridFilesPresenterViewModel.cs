using GongSolutions.Wpf.DragDrop;
using XFiler.SDK;

namespace XFiler.Base
{
    public class GridFilesPresenterViewModel : BaseFilesPresenter
    {
        public GridFilesPresenterViewModel(string directoryPathName,
            IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory) :
            base(fileEntityFactory, dropTarget, dragSource, windowFactory, directoryPathName)
        {
        }
    }
}