using GongSolutions.Wpf.DragDrop;

namespace XFiler.DragDrop;

internal sealed class BookmarksDispatcherDropTarget : IBookmarksDispatcherDropTarget
{
    public void DragOver(IDropInfo dropInfo)
    {
        dropInfo.Effects = DragDropEffects.None;
        dropInfo.EffectText = null;
        dropInfo.DestinationText = null;
        dropInfo.DropTargetAdorner = null;
            
        if (dropInfo.TargetItem is IMenuItemViewModel { Route: null } targetItem)
        {
            dropInfo.DropTargetAdorner = typeof(ToItemAdorner);
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.EffectText = $"Переместить в {targetItem.Header}";
        }
        else if (dropInfo.TargetCollection is ICollection<IMenuItemViewModel>)
        {
            dropInfo.DropTargetAdorner = typeof(ToFolderAdorner);
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.EffectText = $"Переместить сюда";
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not IMenuItemViewModel sourceItem)
            return;

        if (dropInfo.DragInfo.SourceCollection is IList<IMenuItemViewModel> sourceCollection)
            sourceCollection.Remove(sourceItem);

        if (dropInfo.TargetItem is IMenuItemViewModel { Route: null } targetItem)
            targetItem.Items.Add(sourceItem);
        else if (dropInfo.TargetCollection is IList<IMenuItemViewModel> targetCollection)
            targetCollection.Insert(dropInfo.InsertIndex, sourceItem);
    }
}