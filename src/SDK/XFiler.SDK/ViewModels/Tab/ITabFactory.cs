using System.IO;

namespace XFiler.SDK;

public interface ITabFactory
{
    ITabItemModel? CreateExplorerTab(DirectoryInfo directoryInfo);

    ITabItemModel? CreateTab(Route route);
            
    ITabItemModel CreateMyComputerTab();
}