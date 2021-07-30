using System.Windows;

namespace XFiler.SDK
{
    public static class Extensions
    {
        public static IExplorerApp ExplorerApp(this Application application)
        {
            return (IExplorerApp)application;
        }
    }
}