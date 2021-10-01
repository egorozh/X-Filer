namespace XFiler.SDK;

public interface IStorage
{
    string BaseDirectory { get; }

    string LogDirectory { get; }

    string DbDirectory { get; }
            
    string Bookmarks { get; }
}