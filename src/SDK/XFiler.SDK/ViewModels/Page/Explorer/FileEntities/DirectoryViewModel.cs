using System.IO;

namespace XFiler.SDK
{
    public class DirectoryViewModel : FileEntityViewModel, IDirectoryModel
    {
        public DirectoryInfo DirectoryInfo => (DirectoryInfo)Info;

        public DirectoryViewModel(IIconLoader iconLoader,
            IClipboardService clipboardService)
            : base(iconLoader, clipboardService)
        {
        }
    }
}