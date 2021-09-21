using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler
{
    internal class NativeFileIconProvider : IIconProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, int size)
        {
            if (route is { Type: RouteType.File })
                return ImageSystem.GetIcon(route.FullName);

            return null;
        }
    }
}
