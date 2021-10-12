using System.IO;

namespace XFiler;

public sealed class TabFactory : ITabFactory
{
    private readonly Func<ITabItemModel> _tabFactory;

    public TabFactory(Func<ITabItemModel> tabFactory) 
        => _tabFactory = tabFactory;

    public ITabItemModel CreateExplorerTab(DirectoryInfo directoryInfo)
        => CreateTab(new DirectoryRoute(directoryInfo));

    public ITabItemModel CreateMyComputerTab()
        => CreateTab(SpecialRoutes.MyComputer);

    public ITabItemModel CreateTab(Route route)
    {
        var tvm = _tabFactory.Invoke();
        tvm.Init(route);
        return tvm;
    }
}