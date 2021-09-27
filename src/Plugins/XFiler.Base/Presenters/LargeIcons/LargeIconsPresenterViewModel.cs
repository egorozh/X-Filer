using GongSolutions.Wpf.DragDrop;
using Serilog;
using XFiler.SDK;

namespace XFiler.Base
{
    public class LargeIconsPresenterViewModel : BaseFilesPresenter
    {
        public override IconSize IconSize => IconSize.ExtraLarge;

        public LargeIconsPresenterViewModel(IFileEntityFactory fileEntityFactory,
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