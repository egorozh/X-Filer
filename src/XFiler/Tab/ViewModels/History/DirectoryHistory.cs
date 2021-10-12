namespace XFiler.History;

internal sealed class DirectoryHistory : IDirectoryHistory
{
    #region Properties

    public bool CanMoveBack => Current.PreviousNode != null;
    public bool CanMoveForward => Current.NextNode != null;

    public DirectoryNode Current { get; private set; } = null!;

    #endregion

    #region Events

    public event EventHandler? HistoryChanged;

    #endregion

    #region Public Methods

    public void Init(Route route) => Current = new DirectoryNode(route);

    public void MoveBack()
    {
        var prev = Current.PreviousNode;

        Current = prev!;

        RaiseHistoryChanged();
    }

    public void MoveForward()
    {
        var next = Current.NextNode;

        Current = next!;

        RaiseHistoryChanged();
    }

    public void Add(Route route)
    {
        var node = new DirectoryNode(route);

        Current.NextNode = node;
        node.PreviousNode = Current;

        Current = node;

        RaiseHistoryChanged();
    }

    #endregion

    #region Private Methods

    private void RaiseHistoryChanged() => HistoryChanged?.Invoke(this, EventArgs.Empty);

    #endregion

    #region Enumerator

    public IEnumerator<DirectoryNode> GetEnumerator()
    {
        yield return Current;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}