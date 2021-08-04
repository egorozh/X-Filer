using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        public Image? Icon { get; set; }

        public MenuItemViewModel(BookmarkItem bookmarkItem,
            ObservableCollection<MenuItemViewModel> children,
            IIconLoader iconLoader,
            ICommand command)
        {
            Path = bookmarkItem.Path;
            CommandParameter = bookmarkItem.Path;
            Items = children;
            Icon = new Image()
            {
                Source = iconLoader.GetIcon(this, 64),
                Stretch = Stretch.Uniform
            };
            
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