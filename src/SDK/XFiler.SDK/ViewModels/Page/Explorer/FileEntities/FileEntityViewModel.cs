using System;
using System.Windows.Media;

namespace XFiler.SDK
{
    public abstract class FileEntityViewModel : BaseViewModel
    {
        protected IIconLoader IconLoader { get; }

        #region Public Properties

        public XFilerRoute Route { get; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string? Group { get; set; }

        public abstract DateTime ChangeDateTime { get; }

        public ImageSource? Icon { get; set; }

        #endregion

        #region Constructor

        protected FileEntityViewModel(XFilerRoute route, IIconLoader iconLoader)
        {
            Name = route.Header;
            FullName = route.FullName;

            Route = route;
            IconLoader = iconLoader;

            Icon = iconLoader.GetIcon(route, 64);
        }

        #endregion


        public abstract string? GetRootName();

        public abstract FileEntityViewModel Clone();
    }
}