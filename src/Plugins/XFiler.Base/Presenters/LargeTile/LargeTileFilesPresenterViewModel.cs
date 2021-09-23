using GongSolutions.Wpf.DragDrop;
using Serilog;
using XFiler.SDK;

namespace XFiler.Base
{
    public class LargeTileFilesPresenterViewModel : BaseFilesPresenter
    {
        public override IconSize IconSize => IconSize.ExtraLarge;

        public LargeTileFilesPresenterViewModel(IFileEntityFactory fileEntityFactory,
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