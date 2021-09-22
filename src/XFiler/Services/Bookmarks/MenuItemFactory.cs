using System.Collections.ObjectModel;
using System.Windows.Input;

namespace XFiler
{
    internal sealed class MenuItemFactory : IMenuItemFactory
    {
        private readonly IIconLoader _iconLoader;
        private readonly IRenameService _renameService;

        public MenuItemFactory(IIconLoader iconLoader, IRenameService renameService)
        {
            _iconLoader = iconLoader;
            _renameService = renameService;
        }

        public MenuItemViewModel CreateItem(
            BookmarkItem bookmarkItem,
            ObservableCollection<IMenuItemViewModel> children,
            ICommand command)
        {
            return new MenuItemViewModel(bookmarkItem, children,  command, _iconLoader, _renameService);
        }
    }
}