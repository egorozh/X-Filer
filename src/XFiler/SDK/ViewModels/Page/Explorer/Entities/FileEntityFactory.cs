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

        public LogicalDriveViewModel CreateLogicalDrive(string logicalDrive, string? @group = null)
            => new(logicalDrive, _iconLoader)
            {
                Group = @group
            };

        public DirectoryViewModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null)
            => new(directoryInfo, _iconLoader)
            {
                Group = @group
            };

        public FileViewModel CreateFile(FileInfo fileInfo, string? @group = null)
            => new(fileInfo, _iconLoader)
            {
                Group = @group
            };
    }
}