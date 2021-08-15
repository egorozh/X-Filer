using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace XFiler.SDK
{
    public abstract class FileEntityViewModel : BaseViewModel, IFileSystemModel, IDisposable
    {
        #region Private Fields

        private IClipboardService _clipboardService;

        #endregion

        #region Public Properties

        public FileSystemInfo Info { get; }

        public XFilerRoute Route { get; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string? Group { get; set; }

        public DateTime ChangeDateTime => Info.LastWriteTime;

        public ImageSource? Icon { get; set; }

        public bool IsCutted { get; set; }

        #endregion

        #region Constructor

        protected FileEntityViewModel(XFilerRoute route, IIconLoader iconLoader, FileSystemInfo info,
            IClipboardService clipboardService)
        {
            _clipboardService = clipboardService;
            Name = route.Header;
            FullName = route.FullName;

            Route = route;
            Info = info;

            Icon = iconLoader.GetIcon(route, 64);

            _clipboardService.ClipboardChanged += ClipboardServiceOnClipboardChanged;
        }

        #endregion

        public virtual void Dispose()
        {
            _clipboardService = null!;
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