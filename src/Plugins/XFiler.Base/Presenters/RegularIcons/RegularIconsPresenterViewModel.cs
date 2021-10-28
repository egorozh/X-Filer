using GongSolutions.Wpf.DragDrop;

namespace XFiler.Base;

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
        IMainCommands mainCommands,
        INaturalStringComparer naturalStringComparer,
        INativeContextMenuLoader nativeContextMenuLoader) :
        base(fileEntityFactory, dropTarget, dragSource, windowFactory, clipboardService,
            reactiveOptions, fileOperations, logger, renameService, mainCommands, naturalStringComparer, nativeContextMenuLoader)
    {
    }
}