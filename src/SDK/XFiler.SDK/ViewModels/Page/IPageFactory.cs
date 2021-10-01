namespace XFiler.SDK;

public interface IPageFactory
{
    IPageModel? CreatePage(XFilerRoute route);
}