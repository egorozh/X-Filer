using Prism.Commands;

namespace XFiler.SDK
{
    public interface IClipboardService
    {
        DelegateCommand<FileEntityViewModel> PasteCommand { get; }
    }
}