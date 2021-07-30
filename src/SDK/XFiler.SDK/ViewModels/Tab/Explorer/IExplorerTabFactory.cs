using System.IO;

namespace XFiler.SDK
{
    public interface IExplorerTabFactory
    {
        IExplorerTabItemViewModel CreateExplorerTab(DirectoryInfo directoryInfo);
        IExplorerTabItemViewModel CreateExplorerTab(string dirPath, string name);
        IExplorerTabItemViewModel CreateRootTab();
    }
}