namespace XFiler.SDK
{
    public interface IPageFactory
    {
        IPageModel? CreatePage(XFilerUrl url);
    }
}