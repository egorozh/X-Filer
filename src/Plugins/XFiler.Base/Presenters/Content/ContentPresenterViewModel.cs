using GongSolutions.Wpf.DragDrop;
using Serilog;
using XFiler.SDK;

namespace XFiler.Base
{
    public class ContentPresenterViewModel : BaseFilesPresenter
    {
        public override IconSize IconSize => IconSize.Small;

        public ContentPresenterViewModel(IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory,
            IClipboardService clipboardService,
            IExplorerOptions explorerOptions,
            IFileOperations fileOperations,
            ILogger logger,
            IRenameService renameService,
            IMainCommands mainCommands) :
            base(fileEntityFactory, dropTarget, dragSource, windowFactory, clipboardService,
                explorerOptions, fileOperations, logger, renameService, mainCommands)
        {
        }
    }
}