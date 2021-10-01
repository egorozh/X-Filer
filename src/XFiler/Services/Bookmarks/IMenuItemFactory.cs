using System.Windows.Input;

namespace XFiler;

internal interface IMenuItemFactory
{
    MenuItemViewModel CreateItem(BookmarkItem bookmarkItem,
        ObservableCollection<IMenuItemViewModel> children, ICommand bookmarkClickCommand);
}