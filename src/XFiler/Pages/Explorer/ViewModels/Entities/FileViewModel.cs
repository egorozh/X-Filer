using System.IO;
using XFiler.SDK;

namespace XFiler
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        public FileInfo FileInfo => (FileInfo)Info;

        public double Size => FileInfo.Length / 1024.0;

        public FileViewModel(IIconLoader iconLoader, 
            IClipboardService clipboardService)
            : base(iconLoader, clipboardService)
        {
        }
    }
}