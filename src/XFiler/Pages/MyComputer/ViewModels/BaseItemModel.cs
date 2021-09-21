using Prism.Commands;
using System.Windows.Media;

namespace XFiler.MyComputer
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
            Icon = iconLoader.GetIcon(route, IconSize.Large);
        }

        public virtual void Dispose()
        {
            Icon = null;
            OpenCommand = null!;
            Route = null!;
        }
    }
}