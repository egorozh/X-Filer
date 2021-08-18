using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace XFiler.SDK
{
    public abstract class FileEntityViewModel : DisposableViewModel, IFileSystemModel
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

        public DateTime ChangeDateTime => Info.LastWriteTime;

        public ImageSource? Icon { get; set; }

        public bool IsCutted { get; private set; }

        public bool IsSystem { get; private set; }

        public bool IsHidden { get; private set; }

        public bool IsCopyProcess { get; private set; }

        #endregion

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