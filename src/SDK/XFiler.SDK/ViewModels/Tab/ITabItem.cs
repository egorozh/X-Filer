namespace XFiler.SDK
{
    public interface ITabItem
    {
        string Header { get; set; }

        bool IsSelected { get; set; }

        bool LogicalIndex { get; set; }
    }
}