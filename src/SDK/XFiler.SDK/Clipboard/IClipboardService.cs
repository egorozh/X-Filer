using Prism.Commands;

namespace XFiler.SDK
{
    public interface IClipboardService
    {   
        DelegateCommand<IFileSystemModel> PasteCommand { get; }
    }   
}