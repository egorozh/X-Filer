using GongSolutions.Wpf.DragDrop;
using XFiler.Pages.DispatcherPage.Views;
using XFiler.SDK;

namespace XFiler.DispatcherPage
{
    internal class BookmarksDispatcherPageModel : BasePageModel
    {
        public IBookmarksManager BookmarksManager { get; }

        public IDragSource DragSource { get; }
        public IBookmarksDispatcherDropTarget DropTarget { get; }

        public BookmarksDispatcherPageModel(IBookmarksManager bookmarksManager, IDragSource dragSource,
            IBookmarksDispatcherDropTarget dropTarget) : base(typeof(BookmarksDispatcherPage),
            SpecialRoutes.BookmarksDispatcher)
        {
            BookmarksManager = bookmarksManager;
            DragSource = dragSource;
            DropTarget = dropTarget;
        }
    }

    public interface IBookmarksDispatcherDropTarget : IDropTarget { }
}