namespace XFiler.SDK;

public interface IPageFactory
{
    IPageModel CreatePage(Route route);
}