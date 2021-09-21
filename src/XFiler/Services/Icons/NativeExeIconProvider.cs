using System.IO;
using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler
{
    internal class NativeExeIconProvider : IIconProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, IconSize size)
        {   
            if (route is { Type: RouteType.File })
            {
                var info = new FileInfo(route.FullName);

                if (info.Extension.ToLower() == ".exe")
                    return ImageSystem.GetIcon(route.FullName, size == IconSize.Large);
            }

            return null;
        }
    }
}