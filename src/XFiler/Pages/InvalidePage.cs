﻿namespace XFiler;

internal class InvalidatePage : IPageModel
{
    public event EventHandler<HyperlinkEventArgs>? GoToUrl;

    public DataTemplate Template { get; }
    public XFilerRoute Route { get; }
        
    public InvalidatePage(XFilerRoute route)
    {
        Route = route;
    }

    public void Dispose()
    {
    }
}