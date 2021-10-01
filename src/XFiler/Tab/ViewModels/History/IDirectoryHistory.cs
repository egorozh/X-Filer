namespace XFiler.History;

internal interface IDirectoryHistory : IEnumerable<DirectoryNode>
{
    bool CanMoveBack { get; }

    bool CanMoveForward { get; }

    DirectoryNode Current { get; }

    event EventHandler HistoryChanged;

    void MoveBack();

    void MoveForward();

    void Add(XFilerRoute route);    
}