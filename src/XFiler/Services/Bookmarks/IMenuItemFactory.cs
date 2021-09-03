﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using XFiler.SDK;

namespace XFiler
{
    internal interface IMenuItemFactory
    {
        MenuItemViewModel CreateItem(BookmarkItem bookmarkItem,
            ObservableCollection<IMenuItemViewModel> children, ICommand bookmarkClickCommand);
    }
}