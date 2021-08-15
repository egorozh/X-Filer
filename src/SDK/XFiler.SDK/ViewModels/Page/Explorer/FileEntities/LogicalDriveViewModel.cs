using System.IO;

namespace XFiler.SDK
{
    public sealed class LogicalDriveViewModel : DirectoryViewModel
    {
        public LogicalDriveViewModel(DirectoryInfo route, IIconLoader iconLoader, IClipboardService clipboardService)
            : base(route, iconLoader, clipboardService)
        {
        }
    }
}