using System.Windows.Input;

namespace XFiler;

internal interface IMenuItemFactory
{
    IMenuItemViewModel? CreateItem(BookmarkItem bookmarkItem,
        ObservableCollection<IMenuItemViewModel> children, ICommand bookmarkClickCommand);
}