using Microsoft.Win32;

namespace XFiler;

internal class RegistryContextMenuLoader : IRegistryContextMenuLoader
{
    public List<RegistryContextMenuModel> AllEntityContextModels { get; } = new();

    public RegistryContextMenuLoader()
    {
        Init();
    }

    public void Init()
    {
        GetContextModels(Registry.ClassesRoot.OpenSubKey("*\\shell"), AllEntityContextModels);
    }

    private static void GetContextModels(RegistryKey? searchSubKey, ICollection<RegistryContextMenuModel> collection)
    {
        if (searchSubKey == null)
            return;

        foreach (var subKeyName in searchSubKey.GetSubKeyNames())
        {
            var r = searchSubKey.OpenSubKey(subKeyName);

            var cm = CreateModel(r);

            if (cm != null)
                collection.Add(cm);
        }
    }

    private static RegistryContextMenuModel? CreateModel(RegistryKey? registryKey)
    {
        if (registryKey == null)
            return null;

        var s = registryKey.GetSubKeyNames();

        const string command = "command";

        if (s.Length == 1 && s[0] == command)
        {
            var name = (string) registryKey.GetValue("", string.Empty);

            var icon = registryKey.GetValue("Icon") as string;

            var commandValue = registryKey.OpenSubKey(command).GetValue("") as string;

            return new RegistryContextMenuModel(name, icon, commandValue);
        }

        return null;
    }
}