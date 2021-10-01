namespace XFiler.SDK;

public class HyperlinkEventArgs
{
    public HyperlinkEventArgs(XFilerRoute route, bool isOpenInNewTab = false)
    {
        Route = route;
        IsOpenInNewTab = isOpenInNewTab;
    }

    public bool IsOpenInNewTab { get;  }

    public XFilerRoute Route { get;  }
}