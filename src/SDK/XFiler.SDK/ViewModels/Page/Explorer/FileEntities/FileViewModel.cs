using System.IO;

namespace XFiler.SDK
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        public FileInfo FileInfo => (FileInfo)Info;

        public double Size => FileInfo.Length / 1024.0;

        public FileViewModel(IIconLoader iconLoader, IClipboardService clipboardService)
            : base(iconLoader, clipboardService)
        {
        }
    }
}