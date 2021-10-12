namespace XFiler.SDK;

public class HyperlinkEventArgs
{
    public HyperlinkEventArgs(Route route, bool isOpenInNewTab = false)
    {
        Route = route;
        IsOpenInNewTab = isOpenInNewTab;
    }

    public bool IsOpenInNewTab { get;  }

    public Route Route { get;  }
}