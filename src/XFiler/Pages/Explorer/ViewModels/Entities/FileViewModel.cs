using System.IO;

namespace XFiler;

public sealed class FileViewModel : FileEntityViewModel
{
    public FileInfo FileInfo => (FileInfo)Info;

    public long Size => FileInfo.Length;

    public FileViewModel(IIconLoader iconLoader, 
        IClipboardService clipboardService,
        IFileTypeResolver fileTypeResolver)
        : base(iconLoader, clipboardService, fileTypeResolver)
    {
    }
}