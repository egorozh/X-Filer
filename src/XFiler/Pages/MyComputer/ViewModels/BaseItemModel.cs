using System;
using System.Windows.Media;
using Prism.Commands;

namespace XFiler.SDK.MyComputer
{
    public abstract class BaseItemModel : BaseViewModel, IDisposable
    {
        public string Name { get; }

        public ImageSource? Icon { get; private set; }

        public XFilerRoute Route { get; private set; }

        public DelegateCommand<XFilerRoute> OpenCommand { get; private set; }

        protected BaseItemModel(XFilerRoute route, IIconLoader iconLoader, DelegateCommand<XFilerRoute> openCommand)
        {
            Route = route;
            OpenCommand = openCommand;
            Name = route.Header;
            Icon = iconLoader.GetIcon(route, 36);
        }

        public virtual void Dispose()
        {
            Icon = null;
            OpenCommand = null!;
            Route = null!;
        }
    }
}