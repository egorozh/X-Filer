namespace XFiler;

public sealed class LogicalDriveViewModel : DirectoryViewModel
{
    public LogicalDriveViewModel(IIconLoader iconLoader, 
        IClipboardService clipboardService,
        IFileTypeResolver fileTypeResolver)
        : base(iconLoader, clipboardService, fileTypeResolver)
    {
    }
}