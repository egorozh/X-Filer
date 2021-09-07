using XFiler.Pages.DispatcherPage.Views;
using XFiler.SDK;

namespace XFiler.DispatcherPage
{
    internal class BookmarksDispatcherPageModel : BasePageModel
    {
        public IBookmarksManager BookmarksManager { get; }

        public BookmarksDispatcherPageModel(IBookmarksManager bookmarksManager)
            : base(typeof(BookmarksDispatcherPage), SpecialRoutes.BookmarksDispatcher)
        {
            BookmarksManager = bookmarksManager;
        }
    }
}