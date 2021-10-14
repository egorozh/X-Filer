namespace XFiler;

internal class RegistryContextMenuModel : BaseViewModel
{
    public string Name { get; }
    public string? IconPath { get; }
    public string? Command { get; }

    public RegistryContextMenuModel(string name, string? iconPath, string? command)
    {
        Name = name;
        IconPath = iconPath;
        Command = command;
    }
}