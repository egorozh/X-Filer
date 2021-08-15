using Prism.Commands;
using WK.Libraries.SharpClipboardNS;
using XFiler.SDK;

namespace XFiler
{
    public class ClipboardService : IClipboardService
    {
        private readonly SharpClipboard _clipboard;

        public DelegateCommand<IFileSystemModel> PasteCommand { get; }

        public ClipboardService()
        {
            _clipboard = new SharpClipboard();

            _clipboard.ClipboardChanged += ClipboardOnClipboardChanged;

            PasteCommand = new DelegateCommand<IFileSystemModel>(OnPaste, CanPaste);
        }

        private void ClipboardOnClipboardChanged(object? sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            PasteCommand.RaiseCanExecuteChanged();
        }

        private bool CanPaste(IFileSystemModel arg) => System.Windows.Clipboard.ContainsFileDropList();

        private void OnPaste(IFileSystemModel entity)
        {
            var files = System.Windows.Clipboard.GetFileDropList();
        }
    }
}