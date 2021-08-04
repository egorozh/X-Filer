﻿using System.Collections.ObjectModel;
using System.Windows.Input;

namespace XFiler.SDK
{
    internal class MenuItemFactory : IMenuItemFactory
    {
        private readonly IIconLoader _iconLoader;

        public MenuItemFactory(IIconLoader iconLoader)
        {
            _iconLoader = iconLoader;
        }

        public MenuItemViewModel CreateItem(
            BookmarkItem bookmarkItem,
            ObservableCollection<MenuItemViewModel> children,
            ICommand command)
        {
            return new MenuItemViewModel(bookmarkItem, children, _iconLoader, command);
        }
    }
}