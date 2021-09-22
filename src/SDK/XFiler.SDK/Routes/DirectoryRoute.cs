using System.IO;

namespace XFiler.SDK
{
    public record DirectoryRoute : XFilerRoute
    {
        public DirectoryInfo Directory { get; }

        public DirectoryRoute(DirectoryInfo directory)
            : base(directory.Name, directory.FullName, RouteType.Directory)
        {
            Directory = directory;
        }
    }
}