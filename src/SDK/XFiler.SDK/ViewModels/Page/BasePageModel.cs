using System;
using System.Windows;

namespace XFiler.SDK;

public abstract class BasePageModel : BaseViewModel, IPageModel
{
    public DataTemplate Template { get; private set; } = null!;

    public Route Route { get; private set; } = null!;

    public event EventHandler<HyperlinkEventArgs>? GoToUrl;

    protected void GoTo(Route route, bool isOpenInNewTab = false)
    {
        GoToUrl?.Invoke(this, new HyperlinkEventArgs(route, isOpenInNewTab));
    }

    protected void Init(DataTemplate template, Route route)
    {
        Template = template;
        Route = route;
    }

    protected void Init(Type pageType, Route route)
    {
        Route = route;
        Template = CreateTemplate(pageType);
    }

    public virtual void Dispose()
    {
    }

    private DataTemplate CreateTemplate(Type pageType) => new()
    {
        DataType = this.GetType(),
        VisualTree = new FrameworkElementFactory(pageType)
    };
}