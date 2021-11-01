using Microsoft.Win32;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Input;
using Vanara.PInvoke;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace XFiler;

internal class WindowsApiContextMenuLoader : INativeContextMenuLoader
{
    #region Private Fields

    private readonly IStorage _storage;
    private readonly IFileOperations _fileOperations;
    private readonly ILogger _logger;
    private ContextMenu? _contextMenu;

    #endregion

    #region Public Properties

    public IReadOnlyList<IAddNewContextMenuModel> AddItems { get; private set; } = null!;

    #endregion

    #region Commands

    public ICommand InvokeCommand { get; }
    public ICommand InvokeAddNewItemsCommand { get; }

    #endregion

    #region Constructor

    public WindowsApiContextMenuLoader(IStorage storage, IFileOperations fileOperations,
        ILogger logger)
    {
        _storage = storage;
        _fileOperations = fileOperations;
        _logger = logger;
        InvokeCommand = new DelegateCommand<object>(OnInvoke);
        InvokeAddNewItemsCommand = new DelegateCommand<object>(CreateNewFile);
    }

    #endregion

    #region Public Methods

    public IReadOnlyList<IRegistryContextMenuModel> CreateMenuItems(IEnumerable<string> selectedItems)
    {
        _contextMenu?.Dispose();

        _contextMenu = ContextMenuOperations.GetContextMenu(selectedItems, FilterMenuItems(false));

        if (_contextMenu != null)
            return _contextMenu.Items
                .Select(c => new NativeContextMenuItem(c))
                .ToList();

        return new List<IRegistryContextMenuModel>();
    }

    public async void Init()
    {
        AddItems = await GetAddNewContextMenuItems();
    }

    #endregion

    #region Private Methods

    private void OnInvoke(object obj)
    {
        if (obj is NativeContextMenuItem item)
            _contextMenu?.InvokeItem(item.ContextMenuItem.Id);
    }

    private async void CreateNewFile(object item)
    {
        if (item is not Tuple<IAddNewContextMenuModel, IFileSystemModel> tuple)
            return;

        var (model, parent) = tuple;

        var parentDirectory = parent.Info.FullName;

        var endFileName = $"{model.Name}{model.Extension}";

        try
        {
            endFileName = model.Template == null
                ? await _fileOperations.CreateFile(parentDirectory, model.Name, model.Extension)
                : await _fileOperations.CreateFileFromTemplate(parentDirectory, model.Name, model.Extension,
                    model.Template);
        }
        catch (Exception e)
        {
            _logger.Error(e, "WindowsApiContextMenuLoader.CreateNewFile");
        }
        finally
        {
            if (model.Data != null)
            {
                await using var fileStream = File.Open(endFileName, FileMode.Append);
                await fileStream.WriteAsync(model.Data, 0, model.Data.Length);
                await fileStream.FlushAsync();
            }
        }
    }

    private static Func<string?, bool> FilterMenuItems(bool showOpenMenu)
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

        bool FilterMenuItemsImpl(string? menuItem)
        {
            return !string.IsNullOrEmpty(menuItem)
                   && (knownItems.Contains(menuItem) ||
                       (!showOpenMenu && menuItem.Equals("open", StringComparison.OrdinalIgnoreCase)));
        }

        return FilterMenuItemsImpl;
    }

    private static string ExtractStringFromDll(string file, int number)
    {
        var lib = Kernel32.LoadLibrary(file);

        StringBuilder result = new(2048);

        User32.LoadString(lib, number, result, result.Capacity);

        Kernel32.FreeLibrary(lib);

        return result.ToString();
    }

    private async Task<List<AddNewContextMenuModel>> GetAddNewContextMenuItems()
    {
        var newMenuItems = new List<AddNewContextMenuModel>();

        var keys = Registry.ClassesRoot.GetSubKeyNames()
            .Where(x => x.StartsWith("."))
            .Where(x => !new[] {".library-ms", ".url", ".lnk"}.Contains(x));

        foreach (var keyName in keys)
        {
            using var key = OpenSubKeySafe(Registry.ClassesRoot, keyName);

            if (key != null)
            {
                var ret = await GetShellNewRegistryEntries(key, key);

                if (ret != null)
                    newMenuItems.Add(ret);
            }
        }

        return newMenuItems;
    }

    private async Task<AddNewContextMenuModel?> GetShellNewRegistryEntries(RegistryKey current, RegistryKey root)
    {
        foreach (var keyName in current.GetSubKeyNames())
        {
            using var key = OpenSubKeySafe(current, keyName);

            if (key == null)
                continue;

            if (keyName == "ShellNew")
                return await ParseShellNewRegistryEntry(key, root);

            var ret = await GetShellNewRegistryEntries(key, root);

            if (ret != null)
                return ret;
        }

        return null;
    }

    private async Task<AddNewContextMenuModel?> ParseShellNewRegistryEntry(RegistryKey key, RegistryKey root)
    {
        if (!key.GetValueNames().Contains("NullFile") &&
            !key.GetValueNames().Contains("ItemName") &&
            !key.GetValueNames().Contains("FileName"))
        {
            return null;
        }

        var extension = root.Name[(root.Name.LastIndexOf('\\') + 1)..];

        var fileName = (string?) key.GetValue("FileName");

        if (!string.IsNullOrEmpty(fileName) && Path.GetExtension(fileName) != extension)
            return null;

        var dataObj = key.GetValue("Data");

        byte[]? data = null;

        if (dataObj != null)
        {
            data = key.GetValueKind("Data") switch
            {
                RegistryValueKind.Binary => (byte[]) dataObj,
                RegistryValueKind.String => Encoding.UTF8.GetBytes((string) dataObj),
                _ => null
            };
        }

        var sampleFile = new FileInfo(Path.Combine(_storage.ExtensionsDirectory, "file" + extension));

        if (!sampleFile.Exists)
            sampleFile.Create();

        var s = await StorageFile.GetFileFromPathAsync(Path.Combine(_storage.ExtensionsDirectory, "file" + extension));

        var displayType = s != null ? s.DisplayType.Replace("\"", "'") : $"file {extension}";

        var thumbnail = s != null
            ? await s.GetThumbnailAsync(mode: ThumbnailMode.ListView, 24, ThumbnailOptions.UseCurrentScale)
            : null;

        var entry = new AddNewContextMenuModel(extension, displayType, data, fileName, thumbnail);

        return entry;
    }

    private static RegistryKey? OpenSubKeySafe(RegistryKey root, string keyName)
    {
        try
        {
            return root.OpenSubKey(keyName);
        }
        catch (SecurityException)
        {
            return null;
        }
    }

    #endregion
}