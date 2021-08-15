using Prism.Commands;

namespace XFiler.SDK
{
    public interface IClipboardService
    {
        DelegateCommand<object> CutCommand { get; }
        DelegateCommand<object> CopyCommand { get; }
        DelegateCommand<IFileSystemModel> PasteCommand { get; }
    }   
}