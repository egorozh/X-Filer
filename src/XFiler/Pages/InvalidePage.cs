namespace XFiler;

internal class InvalidatePage : IPageModel
{
    public event EventHandler<HyperlinkEventArgs>? GoToUrl;

    public DataTemplate Template { get; }
    public Route Route { get; }
        
    public InvalidatePage(Route route)
    {
        Route = route;
    }

    public void Dispose()
    {
    }
}