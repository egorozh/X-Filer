using GongSolutions.Wpf.DragDrop;

namespace XFiler.DragDrop;

internal sealed class XFilerDragHandler : IDragSource
{   
    /// <inheritdoc />
    public void StartDrag(IDragInfo dragInfo)
    {
        var items = dragInfo.SourceItems.Cast<object>().ToList();
          
        if (items.Count > 1)
        {
            dragInfo.Data = items;
        }
        else
        {
            var singleItem = items.FirstOrDefault();

            dragInfo.Data = singleItem is IEnumerable and not string ? items : singleItem;
        }

        dragInfo.Effects = dragInfo.Data != null
            ? DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link
            : DragDropEffects.None;
    }

    /// <inheritdoc />
    public bool CanStartDrag(IDragInfo dragInfo)
    {
        return true;
    }

    /// <inheritdoc />
    public void Dropped(IDropInfo dropInfo)
    {
    }

    /// <inheritdoc />
    public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
    {
        // nothing here
    }

    /// <inheritdoc />
    public void DragCancelled()
    {
    }

    /// <inheritdoc />
    public bool TryCatchOccurredException(Exception exception)
    {
        return false;
    }
}