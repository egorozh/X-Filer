namespace XFiler.SDK
{
    public sealed class LogicalDriveViewModel : DirectoryViewModel
    {
        public LogicalDriveViewModel(IIconLoader iconLoader, 
            IClipboardService clipboardService)
            : base(iconLoader, clipboardService)
        {
        }
    }
}