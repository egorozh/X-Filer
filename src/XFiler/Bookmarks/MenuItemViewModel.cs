using System.Collections.Generic;
using System.Windows.Input;
using XFiler.SDK;

namespace XFiler
{
    /// <summary>
    /// View-Model MenuItem'а 
    /// </summary>
    public class MenuItemViewModel : BaseViewModel, IMenuItemViewModel
    {
        public string Path { get; set; }

        public string Header { get; set; }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public string IconPath { get; set; }

        public IList<MenuItemViewModel> Items { get; set; }

        public MenuItemViewModel(string path)
        {
            Path = path;

            CommandParameter = path;
        }
    }
}