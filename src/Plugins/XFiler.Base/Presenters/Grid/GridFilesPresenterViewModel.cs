﻿using GongSolutions.Wpf.DragDrop;
using System.IO;
using XFiler.SDK;

namespace XFiler.Base
{
    public class GridFilesPresenterViewModel : BaseFilesPresenter
    {
        public GridFilesPresenterViewModel(DirectoryInfo directoryPathName,
            IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory,
            IClipboardService clipboardService) :
            base(fileEntityFactory, dropTarget, dragSource, windowFactory, clipboardService, directoryPathName)
        {
        }
    }
}