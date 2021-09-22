namespace XFiler
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