using GongSolutions.Wpf.DragDrop;

namespace XFiler.DragDrop;

internal sealed class XFilerDragDrop : IDropTarget
{
    #region Private Fields

    private readonly IFileOperations _fileOperations;

    #endregion

    #region Constructor

    public XFilerDragDrop(IFileOperations fileOperations)
    {
        _fileOperations = fileOperations;
    }

    #endregion

    #region Public Methods

    public void DragOver(IDropInfo dropInfo)
    {
        dropInfo.Effects = DragDropEffects.Scroll;
        dropInfo.EffectText = null;
        dropInfo.DestinationText = null;
        dropInfo.DropTargetAdorner = null;

        if (dropInfo.Data is not IDataObject && dropInfo.DragInfo == null)
            return;

        if (!dropInfo.IsSameDragDropContextAsSource)
            return;

        var targetDirectory = dropInfo.GetTarget();

        if (targetDirectory != null && dropInfo.CanDragItems(dropInfo.GetSources(), targetDirectory))
            return;
    }

    public void Drop(IDropInfo dropInfo)
    {
        var targetDirectory = dropInfo.GetTarget();

        if (targetDirectory == null)
            return;

        var sources = dropInfo.GetSources();

        if (!sources.Any())
            return;

        switch (dropInfo.Effects)
        {
            case DragDropEffects.Move:
                _fileOperations.Move(sources, targetDirectory);
                break;
            case DragDropEffects.Copy:
                _fileOperations.Copy(sources, targetDirectory);
                break;
            case DragDropEffects.Link:
                _fileOperations.CreateLink(sources, targetDirectory);
                break;
        }
    }

    #endregion
}