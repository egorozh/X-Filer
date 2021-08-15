using System.IO;
using Prism.Commands;
using WK.Libraries.SharpClipboardNS;
using XFiler.SDK;

namespace XFiler
{
    public class ClipboardService : IClipboardService
    {
        private readonly IFileOperations _fileOperations;
        private readonly SharpClipboard _clipboard;

        public DelegateCommand<IFileSystemModel> PasteCommand { get; }

        public ClipboardService(IFileOperations fileOperations)
        {
            _fileOperations = fileOperations;
            _clipboard = new SharpClipboard();

            _clipboard.ClipboardChanged += ClipboardOnClipboardChanged;

            PasteCommand = new DelegateCommand<IFileSystemModel>(OnPaste, CanPaste);
        }

        private void ClipboardOnClipboardChanged(object? sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            PasteCommand.RaiseCanExecuteChanged();
        }

        private bool CanPaste(IFileSystemModel arg) =>
            System.Windows.Clipboard.ContainsFileDropList() && arg.Info is not FileInfo;

        private void OnPaste(IFileSystemModel entity)
        {
            var files = System.Windows.Clipboard.GetFileDropList();
        }
    }
}