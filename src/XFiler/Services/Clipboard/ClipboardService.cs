using System.Collections.Specialized;
using System.IO;
using WK.Libraries.SharpClipboardNS;

namespace XFiler;

internal sealed class ClipboardService : IClipboardService
{
    #region Private Fields

    private const string DropEffectFormat = "Preferred DropEffect";

    private readonly IFileOperations _fileOperations;

    #endregion

    #region Events

    public event EventHandler<FileClipboardEventArgs>? ClipboardChanged;

    #endregion

    #region Commands

    public DelegateCommand<object> CutCommand { get; }
    public DelegateCommand<object> CopyCommand { get; }
    public DelegateCommand<object> PasteCommand { get; }

    #endregion

    #region Constructor

    public ClipboardService(IFileOperations fileOperations)
    {
        _fileOperations = fileOperations;

        CutCommand = new DelegateCommand<object>(OnCut);
        CopyCommand = new DelegateCommand<object>(OnCopy);
        PasteCommand = new DelegateCommand<object>(OnPaste, CanPaste);

        SharpClipboard clipboard = new();
        clipboard.ClipboardChanged += ClipboardOnClipboardChanged;
    }

    #endregion

    #region Public Methods

    public bool IsCutted(FileSystemInfo info)
    {
        var items = Clipboard.GetFileDropList()
            .OfType<string>();

        var action = GetAction();

        if (action.HasFlag(DragDropEffects.Move))
        {
            if (items.Any(fi => fi == info.FullName))
                return true;
        }

        return false;
    }

    #endregion

    #region Private Methods

    private static void OnCut(object parameter)
    {
        switch (parameter)
        {
            case IFileSystemModel model:
                CopyOrMove(new[] { model.Info }, DragDropEffects.Move);
                break;
            case IEnumerable e:
                CopyOrMove(e.OfType<IFileSystemModel>().Select(m => m.Info), DragDropEffects.Move);
                break;
        }
    }

    private static void OnCopy(object parameter)
    {
        switch (parameter)
        {
            case IFileSystemModel model:
                CopyOrMove(new[] { model.Info });
                break;
            case IEnumerable e:
                CopyOrMove(e.OfType<IFileSystemModel>().Select(m => m.Info));
                break;
        }
    }

    private static bool CanPaste(object model)
    {
        return Clipboard.ContainsFileDropList();
    }

    private void OnPaste(object model)
    {
        IFileSystemModel? dirModel;

        if (model is object[] { Length: 2 } parameters)
            dirModel = parameters.OfType<IFileSystemModel>().FirstOrDefault();
        else
            dirModel = model as IFileSystemModel;

        if (dirModel is not { Info: DirectoryInfo directory })
            return;

        var items = Clipboard.GetFileDropList()
            .OfType<string>()
            .Select(p => p.ToInfo())
            .OfType<FileSystemInfo>().ToList();

        var action = GetAction();

        if (action.HasFlag(DragDropEffects.Copy))
            _fileOperations.Copy(items, directory);
        else if (action.HasFlag(DragDropEffects.Move))
            _fileOperations.Move(items, directory);
    }

    private void ClipboardOnClipboardChanged(object? sender, SharpClipboard.ClipboardChangedEventArgs e)
    {
        var items = Clipboard.GetFileDropList()
            .OfType<string>()
            .Select(p => p.ToInfo())
            .OfType<FileSystemInfo>().ToList();

        var action = GetAction();

        PasteCommand.RaiseCanExecuteChanged();

        ClipboardChanged?.Invoke(this, new FileClipboardEventArgs(action, items));
    }

    private static void CopyOrMove(IEnumerable<FileSystemInfo> items,
        DragDropEffects effects = DragDropEffects.Copy)
    {
        StringCollection paths = new();
        paths.AddRange(items.Select(i => i.FullName).ToArray());

        var data = new DataObject();
        data.SetFileDropList(paths);
        data.SetData(DropEffectFormat, ToData(effects));
        Clipboard.SetDataObject(data);
    }

    private static DragDropEffects GetAction()
    {
        if (Clipboard.GetData(DropEffectFormat) is not MemoryStream stream)
            return DragDropEffects.None;

        byte[] aMoveEffect = new byte[4];
        stream.Read(aMoveEffect, 0, aMoveEffect.Length);
        return (DragDropEffects)BitConverter.ToInt32(aMoveEffect, 0);
    }

    private static MemoryStream ToData(DragDropEffects effects)
        => new(BitConverter.GetBytes((int)effects));

    #endregion
}