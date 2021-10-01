﻿using GongSolutions.Wpf.DragDrop;

namespace XFiler.Base;

public class GridFilesPresenterViewModel : BaseFilesPresenter
{
    public override IconSize IconSize => IconSize.Small;

    public GridFilesPresenterViewModel(
        IFileEntityFactory fileEntityFactory,
        IDropTarget dropTarget,
        IDragSource dragSource,
        IWindowFactory windowFactory,
        IClipboardService clipboardService,
        IReactiveOptions reactiveOptions,
        IFileOperations fileOperations,
        ILogger logger,
        IRenameService renameService,
        IMainCommands mainCommands,
        INaturalStringComparer naturalStringComparer) :
        base(fileEntityFactory, dropTarget, dragSource, windowFactory, clipboardService, 
            reactiveOptions, fileOperations, logger, renameService, mainCommands, naturalStringComparer)
    {
    }
}