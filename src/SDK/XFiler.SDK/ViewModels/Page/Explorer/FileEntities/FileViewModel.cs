using System.IO;

namespace XFiler.SDK
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        public FileInfo FileInfo { get; }

        public double Size => FileInfo.Length / 1024.0;

        public FileViewModel(FileInfo info, IIconLoader iconLoader, IClipboardService clipboardService)
            : base(new XFilerRoute(info), iconLoader, info, clipboardService)
        {
            FileInfo = info;
        }
    }
}