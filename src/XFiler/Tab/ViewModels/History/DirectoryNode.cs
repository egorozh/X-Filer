namespace XFiler.History;

internal sealed class DirectoryNode
{
    public Route Route { get; }

    public DirectoryNode? PreviousNode { get; set; }
    public DirectoryNode? NextNode { get; set; }
        
    public DirectoryNode(Route route)
    {
        Route = route;
    }
}