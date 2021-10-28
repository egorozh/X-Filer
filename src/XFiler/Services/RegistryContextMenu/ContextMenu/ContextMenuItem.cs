using System.Drawing;

namespace XFiler;

public class ContextMenuItem : IDisposable
{
    public int Id { get; }
    public Bitmap? Icon { get; }
    public string Label { get; }
    public string CommandString { get; }
    public MenuItemType Type { get; }
    public IReadOnlyList<ContextMenuItem>? SubItems { get; private set; }

    public ContextMenuItem(int id, string label,
        string commandString, MenuItemType type,
        Bitmap? icon = null,
        IReadOnlyList<ContextMenuItem>? subItems = null)
    {
        Id = id;
        Icon = icon;
        Label = label;
        CommandString = commandString;
        Type = type;
        SubItems = subItems;
    }

    public void Dispose()
    {
        if (SubItems == null)
            return;

        foreach (var si in SubItems)
            si?.Dispose();

        SubItems = null;
    }
}