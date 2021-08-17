using GongSolutions.Wpf.DragDrop;
using XFiler.SDK;

namespace XFiler.Base
{
    public class GridFilesPresenterViewModel : BaseFilesPresenter
    {
        public GridFilesPresenterViewModel(
            IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory,
            IClipboardService clipboardService,
            IExplorerOptions explorerOptions,
            IFileOperations fileOperations) :
            base(fileEntityFactory, dropTarget, dragSource, windowFactory, clipboardService, 
                explorerOptions, fileOperations)
        {
        }
    }
}