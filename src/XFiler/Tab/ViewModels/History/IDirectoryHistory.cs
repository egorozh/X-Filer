namespace XFiler.History;

internal interface IDirectoryHistory : IEnumerable<DirectoryNode>
{
    bool CanMoveBack { get; }

    bool CanMoveForward { get; }

    DirectoryNode Current { get; }

    event EventHandler HistoryChanged;

    void Init(Route route);

    void Add(Route route);

    void MoveBack();

    void MoveForward();
}