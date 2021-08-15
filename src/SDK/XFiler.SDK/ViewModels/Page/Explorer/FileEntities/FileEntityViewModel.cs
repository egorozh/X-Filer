using System;
using System.IO;
using System.Windows.Media;

namespace XFiler.SDK
{
    public abstract class FileEntityViewModel : BaseViewModel, IFileSystemModel
    {
        protected IIconLoader IconLoader { get; }

        #region Public Properties

        public FileSystemInfo Info { get; }

        public XFilerRoute Route { get; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string? Group { get; set; }

        public DateTime ChangeDateTime => Info.LastWriteTime;

        public ImageSource? Icon { get; set; }

        #endregion

        #region Constructor

        protected FileEntityViewModel(XFilerRoute route, IIconLoader iconLoader, FileSystemInfo info)
        {
            Name = route.Header;
            FullName = route.FullName;

            Route = route;
            IconLoader = iconLoader;
            Info = info;

            Icon = iconLoader.GetIcon(route, 64);
        }

        #endregion
    }
}