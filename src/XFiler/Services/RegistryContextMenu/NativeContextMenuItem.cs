namespace XFiler;

internal class NativeContextMenuItem : IRegistryContextMenuModel
{
    public ContextMenuItem ContextMenuItem { get; }

    public string Name { get; }

    public IReadOnlyList<IRegistryContextMenuModel>? Children { get; }
        
    public NativeContextMenuItem(ContextMenuItem contextMenuItem)
    {
        ContextMenuItem = contextMenuItem;

        Name = contextMenuItem.Label.Replace("&", "");

        if (ContextMenuItem.SubItems != null)
        {
            Children = ContextMenuItem.SubItems
                .Select(subItem => new NativeContextMenuItem(subItem))
                .ToList();
        }
    }
}