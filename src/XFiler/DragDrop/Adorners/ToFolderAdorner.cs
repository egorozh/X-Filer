using GongSolutions.Wpf.DragDrop;
using System.Windows.Media;

namespace XFiler.DragDrop;

public sealed class ToFolderAdorner : DropTargetInsertionAdorner
{
    public ToFolderAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement,
        dropInfo)
    {
        Pen = new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 220, 255)), 2);
    }
}