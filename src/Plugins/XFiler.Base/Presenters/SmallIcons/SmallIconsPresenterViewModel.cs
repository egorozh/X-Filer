using GongSolutions.Wpf.DragDrop;

namespace XFiler.Base;

public class SmallIconsPresenterViewModel : BaseFilesPresenter
{
    public override IconSize IconSize => IconSize.Small;

    public SmallIconsPresenterViewModel(IFileEntityFactory fileEntityFactory,
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