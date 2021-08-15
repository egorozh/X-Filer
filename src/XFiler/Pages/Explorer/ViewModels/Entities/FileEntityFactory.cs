using System.IO;
using XFiler.SDK;

namespace XFiler
{
    public class FileEntityFactory : IFileEntityFactory
    {
        private readonly IIconLoader _iconLoader;
        private readonly IClipboardService _clipboardService;

        public FileEntityFactory(IIconLoader iconLoader, IClipboardService clipboardService)
        {
            _iconLoader = iconLoader;
            _clipboardService = clipboardService;
        }

        public DirectoryViewModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null)
            => new(directoryInfo, _iconLoader, _clipboardService)
            {
                Group = @group
            };

        public FileViewModel CreateFile(FileInfo fileInfo, string? @group = null)
            => new(fileInfo, _iconLoader, _clipboardService)
            {
                Group = @group
            };
    }
}