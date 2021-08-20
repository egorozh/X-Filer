using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using XFiler.SDK;

namespace XFiler
{
    internal class MenuItemViewModel : BaseViewModel, IMenuItemViewModel
    {
        public string? Path { get; set; }

        public string? Header { get; set; }

        public ICommand? Command { get; set; }

        public XFilerRoute? Url { get; }

        public IList<IMenuItemViewModel> Items { get; set; }

        public IIconLoader IconLoader { get; }

        public MenuItemViewModel(BookmarkItem bookmarkItem,
            ObservableCollection<IMenuItemViewModel> children,
            ICommand command, IIconLoader iconLoader)
        {
            Path = bookmarkItem.Path;

            Items = children;
            IconLoader = iconLoader;

            if (Path == null)
            {
                Header = bookmarkItem.BookmarkFolderName;
            }
            else
            {
                Command = command;
                Url = XFilerRoute.FromPath(Path);
                Header = Url.Header;
            }
        }
    }
}