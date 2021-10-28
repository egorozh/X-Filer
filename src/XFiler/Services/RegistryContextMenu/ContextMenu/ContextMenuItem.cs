namespace XFiler;

public class ContextMenuItem : IDisposable
{
    public string IconBase64 { get; set; }
    public int Id { get; set; } // Valid only in current menu to invoke item
    public string? Label { get; set; }  
    public string CommandString { get; set; }
    public MenuItemType Type { get; set; }
    public IReadOnlyList<ContextMenuItem>? SubItems { get; set; }

    public ContextMenuItem()
    {
        SubItems = new List<ContextMenuItem>();
    }

    public void Dispose()
    {
        if (SubItems != null)
        {
            foreach (var si in SubItems)
            {
                (si as IDisposable)?.Dispose();
            }

            SubItems = null;
        }
    }
}