using System.Windows.Media;

namespace XFiler.MyComputer;

public abstract class BaseItemModel : BaseViewModel, IDisposable
{
    public string Name { get; }

    public ImageSource? Icon { get; private set; }

    public Route Route { get; private set; }

    public DelegateCommand<Route> OpenCommand { get; private set; }

    protected BaseItemModel(Route route, IIconLoader iconLoader, DelegateCommand<Route> openCommand)
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