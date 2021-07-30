using System.Windows;
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop;

namespace XFiler.DragDrop
{
    public class ChromerDropTargetInsertAdorner : DropTargetInsertionAdorner
    {
        public ChromerDropTargetInsertAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement,
            dropInfo)
        {
            Pen = new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 220, 255)), 2);
        }
    }
}