using InplaceEditBoxLib.Events;
using InplaceEditBoxLib.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using UserNotification.Events;

namespace XFiler.SDK
{
    public abstract class FileEntityViewModel : DisposableViewModel, IFileSystemModel, IEditBox
    {
        #region Private Fields

        private readonly IIconLoader _iconLoader;
        private IClipboardService _clipboardService;

        #endregion

        #region Public Properties

        public FileSystemInfo Info { get; private set; } = null!;

        public XFilerRoute Route { get; private set; } = null!;

        public string Name { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Group { get; set; }

        public DateTime ChangeDateTime => Info?.LastWriteTime ?? DateTime.MinValue;

        public ImageSource? Icon { get; set; }

        public bool IsCutted { get; private set; }

        public bool IsSystem { get; private set; }

        public bool IsHidden { get; private set; }

        public bool IsCopyProcess { get; private set; }

        #endregion

        public event ShowNotificationEventHandler? ShowNotificationMessage;

        public event RequestEditEventHandler? RequestEdit;

        #region Constructor

        protected FileEntityViewModel(
            IIconLoader iconLoader,
            IClipboardService clipboardService)
        {
            _iconLoader = iconLoader;
            _clipboardService = clipboardService;

            _clipboardService.ClipboardChanged += ClipboardServiceOnClipboardChanged;
        }

        #endregion

        public virtual void Init(XFilerRoute route, FileSystemInfo info)
        {
            Name = route.Header;
            FullName = route.FullName;

            Route = route;
            Info = info;

            Icon = _iconLoader.GetIcon(route, 64);

            IsCutted = _clipboardService.IsCutted(info);
            IsSystem = info.Attributes.HasFlag(FileAttributes.System);
            IsHidden = info.Attributes.HasFlag(FileAttributes.Hidden);
            //IsCopyProcess = info.Attributes.HasFlag(FileAttributes.Archive) && info is FileInfo;
        }

        public void FileSystemInfoChanged(FileSystemInfo? info)
        {
            switch (info)
            {
                case DirectoryInfo directoryInfo:
                    Init(new XFilerRoute(directoryInfo), info);
                    break;
                case FileInfo fileInfo:
                    Init(new XFilerRoute(fileInfo), info);
                    break;
            }
        }

        public void StartRename() => RequestEdit
            ?.Invoke(this, new RequestEdit(RequestEditEvent.StartEditMode));

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                _clipboardService.ClipboardChanged -= ClipboardServiceOnClipboardChanged;

                _clipboardService = null!;
            }

            base.Dispose(disposing);
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
    }
}