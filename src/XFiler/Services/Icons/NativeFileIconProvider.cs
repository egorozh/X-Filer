using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler
{
    internal class NativeFileIconProvider : IIconProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, IconSize size) => route is { Type: RouteType.File }
            ? ImageSystem.GetIcon(route.FullName, size == IconSize.Small)
            : null;
    }
}