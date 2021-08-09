using System;
using System.Windows.Media;
using Prism.Commands;

namespace XFiler.SDK
{
    public class FolderItemModel : BaseViewModel, IDisposable
    {
        public string Name { get; }

        public ImageSource? Icon { get; private set; }

        public XFilerRoute Route { get; private set; }

        public DelegateCommand<XFilerRoute> OpenCommand { get; private set; }

        public FolderItemModel(XFilerRoute route, IIconLoader iconLoader, DelegateCommand<XFilerRoute> openCommand)
        {
            Route = route;
            OpenCommand = openCommand;
            Name = route.Header;
            Icon = iconLoader.GetIcon(route, 36);
        }

        public void Dispose()
        {
            Icon = null;
            OpenCommand = null!;
            Route = null!;
        }
    }
}