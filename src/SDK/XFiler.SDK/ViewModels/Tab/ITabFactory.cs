using System.IO;

namespace XFiler.SDK
{
    public interface ITabFactory
    {
        ITabItemModel CreateExplorerTab(DirectoryInfo directoryInfo);
        ITabItemModel CreateTab(XFilerUrl url);
        ITabItemModel CreateMyComputerTab();    
    }
}   