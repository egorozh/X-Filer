using GongSolutions.Wpf.DragDrop;
using Serilog;
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
            IFileOperations fileOperations,
            ILogger logger,
            IRenameService renameService) :
            base(fileEntityFactory, dropTarget, dragSource, windowFactory, clipboardService, 
                explorerOptions, fileOperations, logger, renameService)
        {
        }
    }
}