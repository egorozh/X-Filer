namespace XFiler.SDK
{
    public interface IStorage
    {
        string BaseDirectory { get; }

        string LogDirectory { get; }

        string Bookmarks { get; }
    }
}