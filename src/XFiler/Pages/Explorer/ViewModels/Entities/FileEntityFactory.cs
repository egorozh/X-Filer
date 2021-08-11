using System.IO;

namespace XFiler.SDK
{
    public class FileEntityFactory : IFileEntityFactory
    {
        private readonly IIconLoader _iconLoader;

        public FileEntityFactory(IIconLoader iconLoader)
        {
            _iconLoader = iconLoader;
        }

        public DirectoryViewModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null)
            => new(new XFilerRoute(directoryInfo), _iconLoader)
            {
                Group = @group
            };

        public FileViewModel CreateFile(FileInfo fileInfo, string? @group = null)
            => new(new XFilerRoute(fileInfo), _iconLoader)
            {
                Group = @group
            };
    }
}