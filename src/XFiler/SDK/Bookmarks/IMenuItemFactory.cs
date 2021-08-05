using System.Collections.ObjectModel;
using System.Windows.Input;

namespace XFiler.SDK
{
    internal interface IMenuItemFactory
    {
        MenuItemViewModel CreateItem(BookmarkItem bookmarkItem,
            ObservableCollection<IMenuItemViewModel> children, ICommand bookmarkClickCommand);
    }
}