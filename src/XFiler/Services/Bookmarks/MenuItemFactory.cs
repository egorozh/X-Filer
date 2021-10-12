using System.Windows.Input;

namespace XFiler;

internal sealed class MenuItemFactory : IMenuItemFactory
{
    private readonly Func<IMenuItemViewModel> _menuFactory;

    public MenuItemFactory(Func<IMenuItemViewModel> menuFactory) => _menuFactory = menuFactory;

    public IMenuItemViewModel? CreateItem(
        BookmarkItem bookmarkItem,
        ObservableCollection<IMenuItemViewModel> children,
        ICommand command)
    {
        var path = bookmarkItem.Path;

        if (path is null)
        {
            var folderName = bookmarkItem.BookmarkFolderName;

            if (!string.IsNullOrWhiteSpace(folderName))
            {
                var model = _menuFactory.Invoke();

                model.Init(folderName, children);

                return model;
            }
        }
        else
        {
            var route = Route.FromPath(path);

            if (route is not null)
            {
                var model = _menuFactory.Invoke();

                model.Init(route, children, command);

                return model;
            }
        }

        return null;
    }
}