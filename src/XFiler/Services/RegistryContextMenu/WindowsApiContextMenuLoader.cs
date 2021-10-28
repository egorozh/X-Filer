using System.Text;
using System.Windows.Input;
using Vanara.PInvoke;

namespace XFiler;

internal class WindowsApiContextMenuLoader : INativeContextMenuLoader
{
    private ContextMenu? _contextMenu;

    public ICommand InvokeCommand { get; }

    public WindowsApiContextMenuLoader()
    {
        InvokeCommand = new DelegateCommand<object>(OnInvoke);
    }

    public IReadOnlyList<IRegistryContextMenuModel> CreateMenuItems(IEnumerable<string> selectedItems)
    {
        _contextMenu = ContextMenuOperations.GetContextMenu(selectedItems, FilterMenuItems(false));

        if (_contextMenu != null)
            return _contextMenu.Items
                .Select(c => new NativeContextMenuItem(c))
                .ToList();

        return new List<IRegistryContextMenuModel>();
    }

    public void DisposeContextMenu()
    {
        _contextMenu?.Dispose();
        _contextMenu = null;
    }

    private void OnInvoke(object obj)
    {
        if (obj is NativeContextMenuItem item)
            _contextMenu?.InvokeItem(item.ContextMenuItem.Id);
    }

    private static Func<string, bool> FilterMenuItems(bool showOpenMenu)
    {
        var knownItems = new List<string>()
        {
            "opennew", "opencontaining", "opennewprocess",
            "runas", "runasuser", "pintohome", "PinToStartScreen",
            "cut", "copy", "paste", "delete", "properties", "link",
            "Windows.ModernShare", "Windows.Share", "setdesktopwallpaper",
            "eject", "rename", "explore", "openinfiles", "extract",
            "copyaspath", "undelete", "empty",
            ExtractStringFromDll("shell32.dll", 30312), // SendTo menu
            ExtractStringFromDll("shell32.dll", 34593), // Add to collection
        };

        bool FilterMenuItemsImpl(string menuItem)
        {
            return !string.IsNullOrEmpty(menuItem)
                   && (knownItems.Contains(menuItem) ||
                       (!showOpenMenu && menuItem.Equals("open", StringComparison.OrdinalIgnoreCase)));
        }

        return FilterMenuItemsImpl;
    }

    public static string ExtractStringFromDll(string file, int number)
    {
        var lib = Kernel32.LoadLibrary(file);

        StringBuilder result = new(2048);

        User32.LoadString(lib, number, result, result.Capacity);

        Kernel32.FreeLibrary(lib);

        return result.ToString();
    }
}