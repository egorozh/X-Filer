using System.Windows;
using GongSolutions.Wpf.DragDrop;

namespace XFiler.DragDrop
{
    internal class BookmarksDispatcherDropTarget : IBookmarksDispatcherDropTarget
    {
        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
           
        }
    }
}