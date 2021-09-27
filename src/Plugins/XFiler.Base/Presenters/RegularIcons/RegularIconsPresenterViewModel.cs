using GongSolutions.Wpf.DragDrop;
using Serilog;
using XFiler.SDK;

namespace XFiler.Base
{
    public class RegularIconsPresenterViewModel : BaseFilesPresenter
    {
        public RegularIconsPresenterViewModel(IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory,
            IClipboardService clipboardService,
            IReactiveOptions reactiveOptions,
            IFileOperations fileOperations,
            ILogger logger,
            IRenameService renameService,
            IMainCommands mainCommands) :
            base(fileEntityFactory, dropTarget, dragSource, windowFactory, clipboardService,
                reactiveOptions, fileOperations, logger, renameService, mainCommands)
        {
        }
    }
}