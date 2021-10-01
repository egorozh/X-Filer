using System.IO;

namespace XFiler;

public class DirectoryViewModel : FileEntityViewModel, IDirectoryModel
{
    public DirectoryInfo DirectoryInfo => (DirectoryInfo)Info;

    public DirectoryViewModel(IIconLoader iconLoader,
        IClipboardService clipboardService,
        IFileTypeResolver fileTypeResolver)
        : base(iconLoader, clipboardService, fileTypeResolver)
    {
    }
}