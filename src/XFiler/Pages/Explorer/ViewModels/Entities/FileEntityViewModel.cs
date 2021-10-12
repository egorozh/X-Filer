using System.IO;
using System.Windows.Media;
using Windows.ImageOperations;
using XFiler.Controls;

namespace XFiler;

public abstract class FileEntityViewModel : DisposableViewModel, IFileSystemModel, IEditBoxModel, IFileItem
{
    #region Private Fields

    private IIconLoader _iconLoader;
    private IClipboardService _clipboardService;
    private IFileTypeResolver _fileTypeResolver;

    #endregion

    #region Public Properties

    public string Type { get; protected set; } = null!;

    public FileSystemInfo Info { get; private set; } = null!;

    public Route Route { get; private set; } = null!;

    public string Name { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Group { get; set; }

    public DateTime ChangeDateTime => Info?.LastWriteTime ?? DateTime.MinValue;

    public ImageSource? Icon { get; set; }

    public bool IsCutted { get; private set; }

    public bool IsSystem { get; private set; }

    public bool IsHidden { get; private set; }

    public bool IsCopyProcess { get; private set; }

    public IFilesGroup FilesGroup { get; private set; } = null!;

    public IconSize IconSize { get; private set; }

    #endregion

    #region Events

    public event EventHandler? RequestEdit;

    #endregion

    #region Constructor

    protected FileEntityViewModel(
        IIconLoader iconLoader,
        IClipboardService clipboardService,
        IFileTypeResolver fileTypeResolver)
    {
        _iconLoader = iconLoader;
        _clipboardService = clipboardService;
        _fileTypeResolver = fileTypeResolver;

        _clipboardService.ClipboardChanged += ClipboardServiceOnClipboardChanged;
    }

    #endregion

    #region Public Methods

    public async Task Init(Route route, FileSystemInfo info, IFilesGroup filesGroup,
        IconSize iconSize)
    {
        Name = route.Header;
        FullName = route.FullName;
        FilesGroup = filesGroup;
        IconSize = iconSize;
        Route = route;
        Info = info;


        IsCutted = _clipboardService.IsCutted(info);
        IsSystem = info.Attributes.HasFlag(FileAttributes.System);
        IsHidden = info.Attributes.HasFlag(FileAttributes.Hidden);

        Type = _fileTypeResolver.GetFileType(info);

        Group = filesGroup.GetGroup(this);
        //IsCopyProcess = info.Attributes.HasFlag(FileAttributes.Archive) && info is FileInfo;
        await LoadIcon(route, iconSize);
    }

    public async Task InfoChanged(FileSystemInfo? info)
    {
        switch (info)
        {
            case DirectoryInfo directoryInfo:
                await Init(new DirectoryRoute(directoryInfo), info, FilesGroup, IconSize);
                break;
            case FileInfo fileInfo:
                await Init(new FileRoute(fileInfo), info, FilesGroup, IconSize);
                break;
        }
    }

    public void StartRename() => RequestEdit?.Invoke(this, EventArgs.Empty);

    #endregion

    #region Protected Methods

    protected override void Dispose(bool disposing)
    {
        if (!Disposed && disposing)
        {
            _clipboardService.ClipboardChanged -= ClipboardServiceOnClipboardChanged;

            _clipboardService = null!;
            _fileTypeResolver = null!;
            _iconLoader = null!;
            FilesGroup = null!;
        }

        base.Dispose(disposing);
    }

    #endregion

    #region Private Methods

    private async Task LoadIcon(Route route, IconSize iconSize)
    {
        var iconStream = await _iconLoader.GetIconStream(route, iconSize);

        if (iconStream != null)
        {
            Icon = ImageSystem.FromStream(iconStream);
            await iconStream.DisposeAsync();
        }
        else
        {
            Application.Current.Dispatcher.Invoke(() => { Icon = _iconLoader.GetIcon(route, iconSize); });
        }
    }

    private void ClipboardServiceOnClipboardChanged(object? sender, FileClipboardEventArgs e)
    {
        if (e.Action.HasFlag(DragDropEffects.Move))
        {
            if (e.Items.Any(fi => fi.FullName == Info.FullName))
            {
                IsCutted = true;
                return;
            }
        }

        IsCutted = false;
    }

    #endregion
}