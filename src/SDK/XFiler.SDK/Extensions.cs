using System.Windows;

namespace XFiler.SDK
{
    public static class Extensions
    {
        public static IXFilerApp XFilerApp(this Application application)
        {
            return (IXFilerApp)application;
        }
    }
}