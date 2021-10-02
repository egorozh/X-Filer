namespace XFiler.DispatcherPage;

internal sealed class BookmarksDispatcherPageModel : BasePageModel
{
    public IBookmarksManager BookmarksManager { get; }

    public BookmarksDispatcherPageModel(IBookmarksManager bookmarksManager)
    {
        Init(typeof(BookmarksDispatcherPage), SpecialRoutes.BookmarksDispatcher);
        BookmarksManager = bookmarksManager;
    }
}