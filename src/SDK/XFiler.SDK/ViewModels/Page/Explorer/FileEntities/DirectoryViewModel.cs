using System.IO;

namespace XFiler.SDK
{
    public class DirectoryViewModel : FileEntityViewModel, IDirectoryModel
    {
        public DirectoryInfo DirectoryInfo { get; }

        public DirectoryViewModel(DirectoryInfo directoryInfo, IIconLoader iconLoader,
            IClipboardService clipboardService)
            : base(new XFilerRoute(directoryInfo), iconLoader, directoryInfo, clipboardService)
        {
            DirectoryInfo = directoryInfo;
        }
    }
}