using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace XFiler.SDK
{
    /// <summary>
    /// View-Model MenuItem'а 
    /// </summary>
    internal class MenuItemViewModel : BaseViewModel, IMenuItemViewModel
    {
        public string? Path { get; set; }

        public string? Header { get; set; }

        public ICommand? Command { get; set; }

        public object? CommandParameter { get; set; }

        public IList<MenuItemViewModel> Items { get; set; }

        public IIconLoader IconLoader { get; }

        public MenuItemViewModel(BookmarkItem bookmarkItem,
            ObservableCollection<MenuItemViewModel> children,
            ICommand command, IIconLoader iconLoader)
        {
            Path = bookmarkItem.Path;
            CommandParameter = bookmarkItem.Path;
            Items = children;
            IconLoader = iconLoader;

            if (Path == null)
            {
                Header = bookmarkItem.BookmarkFolderName;
            }
            else
            {
                Command = command;

                var attr = File.GetAttributes(Path);

                Header = attr.HasFlag(FileAttributes.Directory)
                    ? new DirectoryInfo(Path).Name
                    : new FileInfo(Path).Name;
            }
        }
    }
}