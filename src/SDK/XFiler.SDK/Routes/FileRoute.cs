using System.IO;

namespace XFiler.SDK;

public record FileRoute : Route
{
    public FileInfo File { get; }

    public FileRoute(FileInfo file) : base(file.Name, file.FullName, RouteType.File)
    {
        File = file;
    }
}