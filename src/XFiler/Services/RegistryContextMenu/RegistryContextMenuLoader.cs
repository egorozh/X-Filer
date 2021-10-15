using System.Diagnostics;
using Microsoft.Win32;

namespace XFiler;

internal class RegistryContextMenuLoader : IRegistryContextMenuLoader
{
    public List<IRegistryContextMenuModel> AllEntityContextModels { get; } = new();
    public DelegateCommand<object> InvokeRegistryCommand { get; }

    public RegistryContextMenuLoader()
    {
        InvokeRegistryCommand = new DelegateCommand<object>(OnInvoke);

        Init();
    }
    
    public void Init()
    {
        GetContextModels(Registry.ClassesRoot.OpenSubKey("*\\shell"), AllEntityContextModels);
        //GetContextModels(Registry.CurrentUser, AllEntityContextModels);
    }

    private static void GetContextModels(RegistryKey? searchSubKey, ICollection<IRegistryContextMenuModel> collection)
    {
        if (searchSubKey == null)
            return;

        foreach (var subKeyName in searchSubKey.GetSubKeyNames())
        {
            var r = searchSubKey.OpenSubKey(subKeyName);

            var cm = CreateModel(r);

            if (cm != null)
                collection.Add(cm);

            //GetContextModels(r, collection);
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

            if (!string.IsNullOrWhiteSpace(name))
                return new RegistryContextMenuModel(name, icon, commandValue);
        }

        return null;
    }

    private void OnInvoke(object obj)
    {
        if (obj is Tuple<IFileSystemModel, string?>(var destination, var command))
        {
            if (!string.IsNullOrWhiteSpace(command))
            {
                Process.Start(command.Replace("%1", destination.Info.FullName));
            }
        }
    }
}